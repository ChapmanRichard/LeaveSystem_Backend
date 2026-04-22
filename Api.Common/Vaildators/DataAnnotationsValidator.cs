using System.ComponentModel.DataAnnotations;

namespace Api.Common.Vaildators
{
    public class DataAnnotationsValidator<T> : IValidator<T>
    {


        public DataAnnotationsValidator()
        {

        }

        public void ValidateObject(T instance)
        {
            var context = new ValidationContext(instance, null, null);

            // Throws an exception when instance is invalid.
            Validator.ValidateObject(instance, context, validateAllProperties: true);
            //Validator.ValidateObject(instance, context);
        }


    }
}
