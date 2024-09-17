#region Usings

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace DNA3.Classes {

    public class Data {

        #region ADO Record State

        //  Set Added - Change Datatable Row State to Added if the current Row State is 'Unchanged'
        public static bool SetAdded(ref DataSet ds, string t) {
            bool ReturnValue = true;
            try {
                // AsEnumerable not supported until Core 3.1
                // ds.Tables[t].AsEnumerable().Where(x => x.RowState == DataRowState.Unchanged).ToList().ForEach(x => x.SetAdded());
                var rows = ds.Tables[t].Rows;
                foreach(DataRow r in rows) {
                    r.SetAdded();
                }
            } catch {
                ReturnValue = false;
            }
            return ReturnValue;
        }

        //  Set Modified - Change Datatable Row State to Modified if the current Row State is 'Unchanged'
        public static bool SetModified(ref DataSet ds, string t) {
            bool ReturnValue = true;
            try {
                // AsEnumerable not supported until Core 3.1
                // ds.Tables[t].AsEnumerable().Where(x => x.RowState == DataRowState.Unchanged).ToList().ForEach(x => x.SetModified());
                var rows = ds.Tables[t].Rows;
                foreach(DataRow r in rows) {
                    r.SetModified();
                }
            } catch {
                ReturnValue = false;
            }
            return ReturnValue;
        }

        #endregion

    }

}
