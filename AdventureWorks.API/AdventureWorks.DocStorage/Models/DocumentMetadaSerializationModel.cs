namespace AdventureWorks.DocStorage.Models
{
    public class DocumentMetadaSerializationModel
    {
        public string Title { get; set; }
        public string DocumentSummary { get; set; }
        public int Owner { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public int Revision { get; set; }
        public int Status { get; set; }
        public string DocumentUrl { get; set; }
    }
}
