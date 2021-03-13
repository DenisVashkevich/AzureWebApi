using System.IO;

namespace AdventureWorks.DocStorage.Models
{
    public class WordDocumentModel
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public Stream FileContent { get; set; }
        public string ContentType { get; set; }
        public int Level { get; set; }
        public string Summary { get; set; }
    }
}
