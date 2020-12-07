using System.Threading.Tasks;

namespace AdventureWorks.DocStorage.Interfaces
{
    public interface IUploadNotificationService
    {
        public Task NotifyOnUploadAsync(string fileName);
    }
}
