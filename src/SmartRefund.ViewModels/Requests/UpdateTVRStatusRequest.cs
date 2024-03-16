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
