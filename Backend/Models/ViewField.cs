using Newtonsoft.Json;

namespace PMMC.Models
{
    /// <summary>
    /// The view field
    /// </summary>
    public class ViewField
    {
        /// <summary>
        /// The view field id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The view field name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// The description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The selection type
        /// </summary>
        public string SelectionType { get; set; }

        /// <summary>
        /// The combo code type
        /// </summary>
        [JsonIgnore]
        public string ComboCodeType { get; set; }

        /// <summary>
        /// The system combo code type
        /// </summary>
        [JsonIgnore]
        public string SystemComboCodeType { get; set; }

        /// <summary>
        /// The desc field 
        /// </summary>
        [JsonIgnore]
        public string DescField { get; set; }

        /// <summary>
        /// The link caption 
        /// </summary>
        [JsonIgnore]
        public string LinkCaption { get; set; }

        /// <summary>
        /// The hidden link field
        /// </summary>
        [JsonIgnore]
        public string HiddenLinkField { get; set; }

        /// <summary>
        /// The integer field link
        /// </summary>
        [JsonIgnore]
        public bool IntegerFieldLink { get; set; }
    }
}