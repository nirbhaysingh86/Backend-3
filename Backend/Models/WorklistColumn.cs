namespace PMMC.Models
{
    /// <summary>
    /// The worklist column
    /// </summary>
    public class WorklistColumn
    {
        /// <summary>
        /// The id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The caption
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// The data field
        /// </summary>
        public string DataField { get; set; }

        /// <summary>
        /// The data width
        /// </summary>
        public int? DataWidth { get; set; }

        /// <summary>
        /// The object name
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// The width
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// The readonly flag
        /// </summary>
        public bool? ReadOnly { get; set; }

        /// <summary>
        /// The number format
        /// </summary>
        public string NumberFormat { get; set; }

        /// <summary>
        /// The back color
        /// </summary>
        public string BackColor { get; set; }

        /// <summary>
        /// The fore color
        /// </summary>
        public string ForeColor { get; set; }

        /// <summary>
        /// The multi select flag
        /// </summary>
        public bool? MultiSelect { get; set; }

        /// <summary>
        /// The alternating rows
        /// </summary>
        public int? AlternatingRows { get; set; }

        /// <summary>
        /// The aggregate
        /// </summary>
        public string Aggregate { get; set; }

        /// <summary>
        /// The auto complete flag
        /// </summary>
        public bool? AutoComplete { get; set; }
    }
}
