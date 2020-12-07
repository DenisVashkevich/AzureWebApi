using System;
using System.Threading.Tasks;
using AdventureWorks.DocStorage.Models;

namespace AdventureWorks.DocStorage.Interfaces
{
    public interface IDocumentStorageService
    {
        public Task<Uri> AddDocumentAsync(WordDocumentModel document);
    }
}
