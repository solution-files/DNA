#region Usings

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;

#endregion

namespace Utilities {

    public class Ado {

        #region Methods

        // Cteate New Account
        public static string CreateNewAccount(string cs, string email, string first, string last) {
            string result;
            int clientid;
            int userid;
            try {

                // Client ID
                var builder = WebApplication.CreateBuilder();
                builder.Configuration.AddJsonFile("appsettings.json");
                builder.Configuration.AddJsonFile(builder.Configuration["App:Configuration"]);
                clientid = int.Parse(builder.Configuration["Default:ClientId"]);

                // User
                using (SqlConnection conn = new(cs)) {
                    using (SqlCommand cmd = new("INSERT INTO [User](ClientId, First, Last, RoleId, StatusId, Persist, Comment) OUTPUT INSERTED.UserId VALUES(@ClientId, @First, @Last, @RoleId, @StatusId, @Persist, @Comment)", conn)) {
                        cmd.Parameters.AddWithValue("@ClientId", clientid);
                        cmd.Parameters.AddWithValue("@First", first);
                        cmd.Parameters.AddWithValue("@Last", last);
                        cmd.Parameters.AddWithValue("@RoleId", GetScalarValue<int>(cs, "RoleId", "Role", "Code", "User"));
                        cmd.Parameters.AddWithValue("@StatusId", GetScalarValue<int>(cs, "StatusId", "Status", "Code", "Active"));
                        cmd.Parameters.AddWithValue("@Persist", "true");
                        cmd.Parameters.AddWithValue("@Comment", "Added automatically from Google Authentication Profile");
                        conn.Open();
                        userid = (int)cmd.ExecuteScalar();
                        if (conn.State == ConnectionState.Open) {
                            conn.Close();
                        }
                    }
                }

                // Login Identity
                using (SqlConnection conn = new(cs)) {
                    using (SqlCommand cmd = new("INSERT INTO [Login](Provider, UserId, Email, Password) VALUES (@Provider, @UserId, @Email, '')", conn)) {
                        cmd.Parameters.AddWithValue("@Provider", "Google");
                        cmd.Parameters.AddWithValue("@UserId", userid);
                        cmd.Parameters.AddWithValue("@Email", email);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        if (conn.State == ConnectionState.Open) {
                            conn.Close();
                        }
                    }
                }

                result = "Success";
            } catch(Exception ex) {
                result = ex.Message;
                Log.Error(ex, ex.Message);
            }
            return result;
        }

        // Insert From SQL
        public static string InsertFromSql(string cs, string[] pms, string sql) {
            string result;
            try {
                using (SqlConnection conn = new(cs)) {
                    using (SqlCommand cmd = new(sql, conn)) {
                        foreach(string pm in pms) {
                            var p = pm.Split("=");
                            cmd.Parameters.AddWithValue(p[0], p[1]);
                        }
                        conn.Open();
                        if (cmd.ExecuteNonQuery() > 0) {
                            result = "Success";
                        } else {
                            result = "Record was not inserted. See error log for details";
                        }
                    }
                }
            } catch (Exception ex) {
                Log.Error(ex, ex.Message);
                result = ex.Message;
            }
            return result;
        }
        
        // List from SQL Query - Returns a list of the specified type for the desired SQL statement.
        public static IList<T> ListFromSql<T>(string cs, string[] pms, string sql) {
            IList<T> result = default;
            try {
                DataTable dt = new("Generic");
                using (SqlConnection conn = new(cs)) {
                    using (SqlCommand cmd = new(sql, conn)) {
                        foreach(string pm in pms) {
                            var p = pm.Split("=");
                            cmd.Parameters.AddWithValue(p[0], p[1]);
                        }
                        using (SqlDataAdapter da = new(cmd)) {
                            da.Fill(dt);
                        }
                    }
                }
                result = ConvertDataTable<T>(dt);
            } catch (Exception ex) {
                Log.Error(ex, ex.Message);
            }
            return result;
        }

        // Convert Datatable
        private static List<T> ConvertDataTable<T>(DataTable dt) {
            List<T> data = new();
            foreach (DataRow row in dt.Rows) {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        // Get Item (From Datarow)
        private static T GetItem<T>(DataRow dr) {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns) {
                foreach (PropertyInfo pro in temp.GetProperties()) {
                    if (pro.Name == column.ColumnName) {
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    } else {
                        continue;
                    }
                }
            }
            return obj;
        }

        // Database Name From Connection String
        public static string DatabaseNameFromConnectionString(string cs) {
            string result;
            try {
                SqlConnection connection = new(cs);
                result = connection.Database;
            } catch (Exception ex) {
                result = "";
                Log.Error(ex, ex.Message);
            }
            return result;
        }

        // Database Exists
        public static bool DatabaseExists(string cs) {
            bool result;
            try {
                using (SqlConnection con = new(cs)) {
                    string database = DatabaseNameFromConnectionString(cs);
                    using (SqlCommand cmd = new($"SELECT 1 AS Result FROM master.sys.databases WHERE ([name] = '{database}' OR [name] = '{database}')", con)) {
                        con.Open();
                        object response = cmd.ExecuteScalar();
                        if (con.State == ConnectionState.Open) {
                            con.Close();
                        }
                        if (response != null) {
                            result = true;
                        } else {
                            result = false;
                        }
                    }
                }
            } catch(Exception ex) {
                result = false;
                Log.Error(ex, ex.Message);
            }
            return result;
        }

        // Database Table Exists
        public static bool DatabaseTableExists(string cs, string table) {
            bool result;
            try {
                string database = DatabaseNameFromConnectionString(cs);
                using (SqlConnection con = new(cs)) {
                    using (SqlCommand cmd = new($"SELECT 1 AS Result FROM sys.tables AS T INNER JOIN sys.schemas AS S ON T.schema_id = S.schema_id WHERE S.Name = '{database}' AND T.Name = '{table}';", con)) {
                        con.Open();
                        object response = cmd.ExecuteScalar();
                        if (con.State == ConnectionState.Open) {
                            con.Close();
                        }
                        if (response != null) {
                            result = true;
                        } else {
                            result = false;
                        }
                    }
                }
            } catch (Exception ex) {
                result = false;
                Log.Error(ex, ex.Message);
            }
            return result;
        }

        // Get Scalar Value - Retrieve an value from the table and attribute specified and convert it to the desired type
        public static T GetScalarValue<T>(string connectionstring, string attribute, string entity, string field, string value) {
            T ReturnValue = default;
            try {
                using (SqlConnection con = new(connectionstring)) {
                    using (SqlCommand cmd = new($"SELECT [{attribute}] FROM [{entity}] WHERE [{field}] = '{value}'", con)) {
                        con.Open();
                        object result = cmd.ExecuteScalar();
                        if (con.State == ConnectionState.Open) {
                            con.Close();
                        }
                        if (result != null) {
                            ReturnValue = (T)result;
                        }
                    }
                }
            } catch(Exception ex) {
                Log.Error(ex, ex.Message);
            }
            return ReturnValue;
        }

        //  Set Added - Change Datatable Row State to Added if the current Row State is 'Unchanged'
        public static bool SetAdded(ref DataSet ds, string t) {
			bool ReturnValue = true;
			try {
				ds.Tables[t].AsEnumerable().Where(x => x.RowState == DataRowState.Unchanged).ToList().ForEach(x => x.SetAdded());
			} catch {
				ReturnValue = false;
			}
			return ReturnValue;
		}

		//  Set Modified - Change Datatable Row State to Modified if the current Row State is 'Unchanged'
		public static bool SetModified(ref DataSet ds, string t) {
			bool ReturnValue = true;
			try {
				ds.Tables[t].AsEnumerable().Where(x => x.RowState == DataRowState.Unchanged).ToList().ForEach(x => x.SetModified());
				//var rows = ds.Tables[t].AsEnumerable().Where(x => x.RowState == DataRowState.Unchanged);
				//foreach (DataRow r in rows) {
				//    if (r.RowState == DataRowState.Unchanged) {
				//        r.SetModified();
				//    }
				//}
			} catch {
				ReturnValue = false;
			}
			return ReturnValue;
		}

        #endregion

    }

}