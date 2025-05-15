#region Usings

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

#endregion

namespace DNA3.Classes {

    public class PaginatedList<T> : List<T> {

        #region Properties and Variables

        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        #endregion

        #region Class Methods

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize) {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public bool HasPreviousPage {
            get {
                return PageIndex > 1;
            }
        }

        public bool HasNextPage {
            get {
                return PageIndex < TotalPages;
            }
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize) {
            PaginatedList<T> returnValue = null;
            try {
                var count = await source.CountAsync();
                var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                returnValue = new PaginatedList<T>(items, count, pageIndex, pageSize);
            } catch (Exception ex) {
                Debug.Print(ex.Message);
            }
            return returnValue;
        }

        #endregion

    }

}