using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.WebAPI.Request
{
    [ExcludeFromCodeCoverage]
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
