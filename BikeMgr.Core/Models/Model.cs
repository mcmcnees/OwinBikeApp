using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BikeMgr.Core.Models
{
    public abstract class Model : IValidatableObject
    {
        public void Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this, serviceProvider: null, items: null));
        }
        public bool TryValidate(out ICollection<ValidationResult> errors)
        {
            errors = new List<ValidationResult>();
            try
            {
                Validate();
                return true;
            }
            catch (ValidationException vex)
            {
                errors.Add(vex.ValidationResult);
                return false;
            }
        }
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();
            return errors;
        }
    }
}
