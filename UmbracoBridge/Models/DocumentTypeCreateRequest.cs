namespace UmbracoBridge.Models
{
    public class DocumentTypeCreateRequest
    {
        public string Alias { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public bool AllowedAsRoot { get; set; } = true;
        public bool VariesByCulture { get; set; } = false;
        public bool VariesBySegment { get; set; } = false;
        public bool IsElement { get; set; } = true;
    }
}
