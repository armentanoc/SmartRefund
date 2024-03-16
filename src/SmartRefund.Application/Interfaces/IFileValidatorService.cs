
using Microsoft.AspNetCore.Http;
using SmartRefund.ViewModels.Responses;

namespace SmartRefund.Application.Interfaces
{
    public interface IFileValidatorService
    {
        Task<InternalReceiptResponse?> Validate(IFormFile file, uint employeeId);
        bool ValidateExtension(string fileName);
        bool ValidateSize(long lenght);
        bool ValidateResolution(MemoryStream memoryStream);
        bool ValidateType(string fileName, MemoryStream memoryStream);
    }
}
