#region Usings

using Serilog;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

#endregion

namespace Utilities {

    public class Ado {

        #region Methods

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
                using (SqlConnection con = new SqlConnection(cs)) {
                    string database = DatabaseNameFromConnectionString(cs);
                    using (SqlCommand cmd = new SqlCommand($"SELECT 1 AS Result FROM master.sys.databases WHERE ([name] = '{database}' OR [name] = '{database}')", con)) {
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
                using (SqlConnection con = new SqlConnection(cs)) {
                    using (SqlCommand cmd = new SqlCommand($"SELECT 1 AS Result FROM sys.tables AS T INNER JOIN sys.schemas AS S ON T.schema_id = S.schema_id WHERE S.Name = '{database}' AND T.Name = '{table}';", con)) {
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
                using (SqlConnection con = new SqlConnection(connectionstring)) {
                    using (SqlCommand cmd = new SqlCommand($"SELECT [{attribute}] FROM [{entity}] WHERE [{field}] = '{value}'", con)) {
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