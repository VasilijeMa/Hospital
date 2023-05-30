using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ZdravoCorp.GUI.ValidationRule
{
    public class NotificationMessageValidationRule : System.Windows.Controls.ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(value.ToString() != "") return ValidationResult.ValidResult;
            return new ValidationResult(false, "Empty field");
        }
    }
}
