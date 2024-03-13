
using Microsoft.AspNetCore.Http;
using SmartRefund.Domain.Models;
using SmartRefund.ViewModels.Responses;

namespace SmartRefund.Application.Interfaces
{
    public interface IFileValidatorService
    {
        public Task<InternalReceiptResponse?> Validate(IFormFile file, uint employeeId);

        public bool ValidateExtension(string fileName);

        public bool ValidateSize(long lenght);
    }
}
