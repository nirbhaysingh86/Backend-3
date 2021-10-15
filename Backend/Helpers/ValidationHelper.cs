using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PMMC.Helpers
{
    /// <summary>
    /// The validation helper
    /// </summary>
    internal static class ValidationHelper
    {
        /// <summary>
        /// Check validation error
        /// </summary>
        /// <param name="this">the object to check</param>
        /// <returns>error messages if exist validation errors</returns>
        public static IEnumerable<string> ValidationErrors(this object @this)
        {
            var context = new ValidationContext(@this, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(@this, context, results, true);
            foreach (var validationResult in results)
            {
                yield return validationResult.ErrorMessage;
            }
        }
    }
}
