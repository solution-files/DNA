#region Usings

using DNA3.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Action = DNA3.Models.Action;

#endregion

namespace DNA3.Classes {

    #region Interface

    public interface ITools {

        Task<int> GetSourceKeyValue(string value);
        Task<int> GetDispositionKeyValue(string value);
        Task<int> GetPageKeyValue(string value);
        Task<int> GetSectionKeyValue(string value);
        Task<int> GetCategoryKeyValue(string value);
        Task<int> GetStatusKeyValue(string value);
        IList<Assembly> GetAssemblyList();

    }

    #endregion

    #region Class

    public class Tools(MainContext context, ILogger<Tools> logger) : ITools {

        #region Services

        // Services
        private MainContext Context { get; set; } = context;
        private ILogger<Tools> Logger { get; set; } = logger;

        #endregion

        #region Methods

        // Get Source Key Value
        public async Task<int> GetSourceKeyValue(string value) {
            int result = 0;
            try {
                Source instance = await Context.Source.Where(x => x.Name == value).FirstOrDefaultAsync();
                if (instance == null) {

                } else {
                    result = instance.SourceId;
                }
            } catch (Exception ex) {
                Logger.LogError(ex, "{message}", ex.Message);
            }
            return result;
        }

        // Get Disposition Key Value
        public async Task<int> GetDispositionKeyValue(string value) {
            int result = 0;
            try {
                Disposition instance = await Context.Disposition.Where(x => x.Name == value).FirstOrDefaultAsync();
                if (instance == null) {

                } else {
                    result = instance.DispositionId;
                }
            } catch (Exception ex) {
                Logger.LogError(ex, "{message}", ex.Message);
            }
            return result;
        }

        // Get Page Key Value
        public async Task<int> GetPageKeyValue(string value) {
            int result = 0;
            try {
                Page instance = await Context.Page.Where(x => x.Name == value).FirstOrDefaultAsync();
                if (instance == null) {

                } else {
                    result = instance.PageId;
                }
            } catch (Exception ex) {
                Logger.LogError(ex, "{message}", ex.Message);
            }
            return result;
        }

        // Get Section Key Value
        public async Task<int> GetSectionKeyValue(string value) {
            int result = 0;
            try {
                Section instance = await Context.Section.Where(x => x.Name == value).FirstOrDefaultAsync();
                if (instance == null) {

                } else {
                    result = instance.SectionId;
                }
            } catch (Exception ex) {
                Logger.LogError(ex, "{message}", ex.Message);
            }
            return result;
        }

        // Get Category Key Value
        public async Task<int> GetCategoryKeyValue(string value) {
            int result = 0;
            try {
                Category instance = await Context.Category.Where(x => x.Name == value).FirstOrDefaultAsync();
                if (instance == null) {

                } else {
                    result = instance.CategoryId;
                }
            } catch (Exception ex) {
                Logger.LogError(ex, "{message}", ex.Message);
            }
            return result;
        }

        // Get Status Key Value
        public async Task<int> GetStatusKeyValue(string value) {
            int result = 0;
            try {
                Status instance = await Context.Status.Where(x => x.Name == value).FirstOrDefaultAsync();
                if (instance == null) {

                } else {
                    result = instance.StatusId;
                }
            } catch (Exception ex) {
                Logger.LogError(ex, "{message}", ex.Message);
            }
            return result;
        }

        // Get Table Key Value
        public async Task<int> GetTableKeyValue(string value) {
            int result = 0;
            try {
                Table instance = await Context.Table.Where(x => x.Name == value).FirstOrDefaultAsync();
                if (instance == null) {

                } else {
                    result = instance.TableId;
                }
            } catch (Exception ex) {
                Logger.LogError(ex, "{message}", ex.Message);
            }
            return result;
        }

        // Get Role Key Value
        public async Task<int> GetRoleKeyValue(string value) {
            int result = 0;
            try {
                Role instance = await Context.Role.Where(x => x.Name == value).FirstOrDefaultAsync();
                if (instance == null) {

                } else {
                    result = instance.RoleId;
                }
            } catch (Exception ex) {
                Logger.LogError(ex, "{message}", ex.Message);
            }
            return result;
        }

        // Get Menu Key Value
        public async Task<int> GetMenuKeyValue(string value) {
            int result = 0;
            try {
                Menu instance = await Context.Menu.Where(x => x.Code == value).FirstOrDefaultAsync();
                if (instance == null) {

                } else {
                    result = instance.MenuId;
                }
            } catch (Exception ex) {
                Logger.LogError(ex, "{message}", ex.Message);
            }
            return result;
        }

        // Get Assembly List
        public IList<Assembly> GetAssemblyList() {
            IList<Assembly> result = default;
            try {
                result = [.. AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.FullName.Contains("Microsoft") && !x.FullName.Contains("System") && !x.FullName.Contains("Serilog") && !x.FullName.Contains("Telerik") && !x.FullName.Contains("Syncfusion") && !x.FullName.Contains("Netstandard") && !x.FullName.Contains("Swashbuckle")).OrderBy(x => x.FullName)];
            } catch (Exception ex) {
                Logger.LogError(ex, "{message}", ex.Message);
            }
            return result;
        }

        #endregion

    }

    #endregion

}