#region Usings

using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

#endregion

namespace Utilities {

    public class Ado {

        #region Methods
        
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
            List<T> data = new List<T>();
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
                IDbConnection connection = new SqlConnection(cs);
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