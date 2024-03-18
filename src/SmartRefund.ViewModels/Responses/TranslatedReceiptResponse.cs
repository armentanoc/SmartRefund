
using SmartRefund.CustomExceptions;
using SmartRefund.Domain.Enums;
using SmartRefund.Domain.Models;
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.ViewModels.Responses
{
    [ExcludeFromCodeCoverage]
    public class TranslatedReceiptResponse
    {
        public string UniqueHash { get; set; }
        public uint EmployeeId { get; set; }
        public decimal Total { get; set; }
        public TranslatedVisionReceiptCategoryEnum Category { get; set; }    
        public TranslatedVisionReceiptStatusEnum Status { get; set; }
        public string Description { get; set; }

        public TranslatedReceiptResponse()
        {
            // required by EF
        }
        public TranslatedReceiptResponse(string uniqueHash, uint employeeId, decimal total, string category, string status, string description)
        {
            UniqueHash = uniqueHash;
            EmployeeId = employeeId;
            Total = total;

            if (Enum.TryParse(category, out TranslatedVisionReceiptCategoryEnum parsedCategory))
                Category = parsedCategory;
            else
                throw new UnableToParseException(category);

            if (Enum.TryParse(status, out TranslatedVisionReceiptStatusEnum parsedStatus))
                Status = parsedStatus;
            else
                throw new UnableToParseException(status);

            Description = description;
        }

        public TranslatedReceiptResponse(TranslatedVisionReceipt translatedVision)
        {
            if(translatedVision is TranslatedVisionReceipt)
            {
                UniqueHash = translatedVision.UniqueHash;
                Total = translatedVision.Total;
                Category = translatedVision.Category;
                Status = translatedVision.Status;
                Description = translatedVision.Description;
            }
        }
    }
}
