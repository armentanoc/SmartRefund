using SmartRefund.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.ViewModels.Requests
{
    [ExcludeFromCodeCoverage]
    public class UpdateTVRStatusRequest
    {
        public string UniqueHash { get; init; }
        public int NewStatus { get; init; }

        public UpdateTVRStatusRequest(string uniqueHash, int newStatus)
        {
            UniqueHash = uniqueHash;
            NewStatus = newStatus;
        }
    }
}
