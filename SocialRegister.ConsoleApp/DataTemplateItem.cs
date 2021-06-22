namespace SocialRegister
{
    public class DataTemplateItem
    {
        /// <summary>
        /// Used in data grouping.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Name of column.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Column width for console output used in formatting.
        /// </summary>
        public int Width { get; set; }
    }
}