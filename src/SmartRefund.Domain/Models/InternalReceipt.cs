using SmartRefund.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.Domain.Models
{
    public class InternalReceipt : BaseEntity
    {
        public uint EmployeeId { get; private set; }
        public DateTime CreationDate { get; private set; }
        public InternalReceiptStatusEnum Status { get; set; }

        //Recebe a imagem

        public InternalReceipt(uint employeeId) 
        {
            CreationDate = DateTime.Now;
            EmployeeId = employeeId;
            Status = InternalReceiptStatusEnum.Unprocessed;
        }
        public InternalReceipt() { }
    }
}
