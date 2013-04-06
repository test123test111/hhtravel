using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace HHTravel.Infrastructure
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class DateAttribute : ValidationAttribute
    {
        public DateAttribute() :
            base("\"{0}\" must be a date without time portion or is empty.")
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture,
                ErrorMessageString, name);
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                if (value.GetType() == typeof(DateTime))
                {
                    DateTime dateTime = (DateTime)value;
                    return dateTime.TimeOfDay == TimeSpan.Zero;
                }
                else if (value.GetType() == typeof(Nullable<DateTime>))
                {
                    DateTime? dateTime = (DateTime?)value;
                    return !dateTime.HasValue
                        || dateTime.Value.TimeOfDay == TimeSpan.Zero;
                }
            }
            return true;
        }
    }
}