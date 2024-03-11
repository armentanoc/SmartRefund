
namespace SmartRefund.Application.Interfaces
{
    public interface IFileValidatorService
    {
        public bool Validate(long lenght, string name, string filePath);
    }
}
