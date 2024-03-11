
using Microsoft.AspNetCore.Http;

namespace SmartRefund.Application.Interfaces
{
    public interface IFileValidatorService
    {
        public bool Validate(IFormFile file);
    }
}
