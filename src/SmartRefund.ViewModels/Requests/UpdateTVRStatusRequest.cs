
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.ViewModels.Requests
{
    [ExcludeFromCodeCoverage]
    public class UpdateTVRStatusRequest
    {
        [Required(ErrorMessage = "UniqueHash is required.")]
        public string UniqueHash { get; init; }

        [Required(ErrorMessage = "New status is required.")]
        public string NewStatus { get; init; }

        public UpdateTVRStatusRequest(string uniqueHash, string newStatus)
        {
            UniqueHash = uniqueHash;
            NewStatus = newStatus;
        }
    }
}
