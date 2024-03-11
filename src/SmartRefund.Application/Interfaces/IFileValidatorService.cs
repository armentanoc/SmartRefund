
using Microsoft.AspNetCore.Http;
using SmartRefund.Domain.Models;

namespace SmartRefund.Application.Interfaces
{
    public interface IFileValidatorService
    {
        public Task<InternalReceipt?> Validate(IFormFile file, uint employeeId);

        public bool ValidateExtension(string fileName);

        public bool ValidateSize(long lenght);
    }
}
