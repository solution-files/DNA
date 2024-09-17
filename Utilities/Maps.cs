#region Usings

using BingMapsRESTToolkit;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

#endregion

namespace Utilities {

    public class Maps {

        #region Geolocation

        // Latitude and Logitude from Address
        public static async Task<string> LatitudeLongitudeFromAddressAsync(string street, string city, string state, string zip) {
            var request = new GeocodeRequest();
            double latitude = 0;
            double longitude = 0;
            request.BingMapsKey = "AmSSsZPqH9eAznme_JJfNzMGy8JTCgsUCd0KmNPy1eCuCC1Qb6Y0sgpvN-dT6BbK";

            request.Address = new SimpleAddress() {
                CountryRegion = "USA",
                AddressLine = street,
                //PostalCode = zip,
                Locality = city,
                AdminDistrict = state
            };

            var result = await request.Execute();
            if (result.StatusCode == 200) {
                var toolkitLocation = (result?.ResourceSets?.FirstOrDefault())
                        ?.Resources?.FirstOrDefault()
                        as BingMapsRESTToolkit.Location;
                latitude = toolkitLocation.Point.Coordinates[0];
                longitude = toolkitLocation.Point.Coordinates[1];
            }
            return $"{latitude.ToString()}:{longitude.ToString()}";
        }

        #endregion

    }

}
