using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.Application.Services
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class AlreadyUpdatedReceiptException : Exception
    {
        public uint Id { get; private set; }
        public string UniqueHash { get; private set; }
        public AlreadyUpdatedReceiptException(uint id)
           : base($"Receipt with id: {id} has already been updated!")
        {
            Id = id;
        }
        public AlreadyUpdatedReceiptException(string uniqueHash)
           : base($"Receipt with Unique Hash: {uniqueHash} has already been updated!")
        {
            UniqueHash = uniqueHash;
        }
    }
}
