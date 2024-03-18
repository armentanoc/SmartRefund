using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.ViewModels.Requests
{
    [ExcludeFromCodeCoverage]
    public class FrontFilter
    {
        public int[] OptionsStatusRefund { get; set; }
        public int[] OptionsStatusTranslate { get; set; }
        public int[] OptionsStatusGPT { get; set; }
        public FrontFilter(int[] optionsStatusRefund, int[] optionsStatusTranslate, int[] optionsStatusGPT)
        {
            OptionsStatusRefund = optionsStatusRefund;
            OptionsStatusTranslate = optionsStatusTranslate;
            OptionsStatusGPT = optionsStatusGPT;
        }
    }
}
