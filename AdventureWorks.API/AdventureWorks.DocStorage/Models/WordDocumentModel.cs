using System.IO;

namespace AdventureWorks.DocStorage.Models
{
    public class WordDocumentModel
    {
        public string FileName { get; set; }
        public Stream FileContent { get; set; }
        public string ContentType { get; set; }
    }
}
