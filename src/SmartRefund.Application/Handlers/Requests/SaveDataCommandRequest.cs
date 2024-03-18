using MediatR;
using SmartRefund.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.Application.Handlers.Requests
{
    [ExcludeFromCodeCoverage]
    public class SaveDataCommandRequest : IRequest<Unit>
    {
        public InternalReceipt Response { get; }

        public SaveDataCommandRequest(InternalReceipt response)
        {
            Response = response;
        }
    }
}
