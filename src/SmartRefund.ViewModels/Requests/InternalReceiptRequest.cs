using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRefund.ViewModels.Requests
{
    [ExcludeFromCodeCoverage]
    internal class InternalReceiptRequest
    {
        [Required(ErrorMessage = "Id de funcionário é obrigatório")]
        public uint Id { get; set; }

        //não estamos usando essa classe! dúvida em como pedir a imagem por aqui ao invés do swagger

    }
}
