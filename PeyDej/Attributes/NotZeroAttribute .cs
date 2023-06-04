using System.ComponentModel.DataAnnotations;

namespace PeyDej.Attributes
{
    public class NotZeroAttribute : ValidationAttribute
    {
        public override bool IsValid(object value) => (long)value != 0;
    }
}
