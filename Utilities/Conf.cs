#region Usings

using Microsoft.Extensions.Configuration;
using System;
using System.IO;

#endregion

namespace Utilities {

    public class Conf {

        #region Methods

        public static T AppSetting<T>(string Key) {
            T ReturnValue = default(T);
            try {
                var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                .AddJsonFile("C:\\DNASettings.json", optional: false, reloadOnChange: true)
                                .AddEnvironmentVariables();

                IConfigurationRoot configuration = builder.Build();

                ReturnValue = (T)Convert.ChangeType(configuration[Key], typeof(T));

            } catch {
                // Do nothing for now
            }
            return ReturnValue;
        }

        #endregion

    }

}