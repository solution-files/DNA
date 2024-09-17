#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DNA3.Models;

#endregion

namespace DNA3.Classes {
    public class CustomEquipment : ValidationAttribute {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            Customer customer = (Customer)validationContext.ObjectInstance;
            string ValidItems = "AIR, AIRMASTER, AMC, AQC, BIGBLUE, CS, EC4, EC5, F8C, HYDROTECH, IRON, OXY, P-6, P-12, PRO, Q2, QRS, RO, TC, TCM, TWIN, UNKNOWN";
            bool Result = true;

            if (!string.IsNullOrEmpty(customer.Equipment)) {
                foreach (string Item in customer.Equipment.Split(",")) {
                    if (!ValidItems.Contains(Item)) {
                        Result = false;
                        break;
                    }
                }
            }

            if (Result == false) {
                return new ValidationResult("Specify " + ValidItems.ToString());
            } else {
                return ValidationResult.Success;
            }
        }

    }
}
