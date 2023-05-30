using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ZdravoCorp.GUI.ValidationRule
{
    public class NotificationMinutesBeforeValidationRule : System.Windows.Controls.ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (Int32.TryParse(value.ToString(), out int minutes))
            {
                if (minutes > 60 * 24 || minutes < 0) return new ValidationResult(false, "Out of range");
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, "Not integer");
        }
    }
}
