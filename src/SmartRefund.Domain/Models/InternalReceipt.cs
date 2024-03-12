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
        public InternalReceiptStatusEnum Status { get; private set; }
        public byte[] Image { get; private set; }

        //Recebe a imagem

        public InternalReceipt(uint employeeId, byte[] image) 
        {
            CreationDate = DateTime.Now;
            EmployeeId = employeeId;
            Image = image;
            Status = InternalReceiptStatusEnum.Unprocessed;
        }
        public InternalReceipt() { }

        public void SetStatus(InternalReceiptStatusEnum status)
        {
            Status = status;
        }
    }
}
