using Newtonsoft.Json;

namespace PMMC.Models
{
    /// <summary>
    /// The field value
    /// </summary>
    public class FieldValue
    {
        /// <summary>
        /// The field value id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The description
        /// </summary>
        [JsonIgnore]
        public string Description { get; set; }

        /// <summary>
        /// The display value
        /// </summary>
        public string DisplayValue { get; set; }
    }
}