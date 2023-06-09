using System.ComponentModel.DataAnnotations;

namespace PeyDej.Attributes
{
    public class NotZeroAttribute : ValidationAttribute
    {
        public override bool IsValid(object value) {
            bool result = false;
            try
            {
                result = (long)value != 0;
            }
            catch
            {
                result = (int)value != 0;
            }
            return result;
        }
    }
}
