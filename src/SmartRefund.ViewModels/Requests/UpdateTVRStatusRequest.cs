
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.ViewModels.Requests
{
    [ExcludeFromCodeCoverage]
    public class UpdateTVRStatusRequest
    {
        [Required(ErrorMessage = "Id is required.")]
        public uint Id { get; init; }

        [Required(ErrorMessage = "New status is required.")]
        public string NewStatus { get; init; }

        public UpdateTVRStatusRequest(string newStatus, uint id)
        {
            NewStatus = newStatus;
            Id = id;
        }
    }
}
