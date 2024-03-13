namespace SmartRefund.Application.Interfaces
{
    public interface IVisionExecutorServiceConfiguration
    {
        string OpenAIVisionApiEnvVar { get; }
        OpenAIVisionPrompts OpenAIVisionPrompts { get; }
    }

    public class OpenAIVisionPrompts
    {
        private string? _systemPrompt;
        public string SystemPrompt
        {
            get => _systemPrompt;
            set => _systemPrompt = ValidatePrompt(value, "System prompt");
        }

        private string? _imagePrompt;
        public string ImagePrompt
        {
            get => _imagePrompt;
            set => _imagePrompt = ValidatePrompt(value, "Image prompt");
        }

        private string? _isReceiptPrompt;
        public string IsReceiptPrompt
        {
            get => _isReceiptPrompt;
            set => _isReceiptPrompt = ValidatePrompt(value, "IsReceipt prompt");
        }

        private string? _totalPrompt;
        public string TotalPrompt
        {
            get => _totalPrompt;
            set => _totalPrompt = ValidatePrompt(value, "Total prompt");
        }

        private string? _categoryPrompt;
        public string CategoryPrompt
        {
            get => _categoryPrompt;
            set => _categoryPrompt = ValidatePrompt(value, "Category prompt");
        }

        private string? _descriptionPrompt;
        public string DescriptionPrompt
        {
            get => _descriptionPrompt;
            set => _descriptionPrompt = ValidatePrompt(value, "Description prompt");
        }

        private string ValidatePrompt(string value, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"Configuration of prompt {propertyName} cannot be null or whitespace");
            return value;
        }
    }
}
