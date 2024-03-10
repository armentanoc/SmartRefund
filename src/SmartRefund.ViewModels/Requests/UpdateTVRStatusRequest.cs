using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.ViewModels.Requests
{
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
