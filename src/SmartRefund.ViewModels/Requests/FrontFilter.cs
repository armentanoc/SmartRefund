
namespace SmartRefund.ViewModels.Requests
{
    public class FrontFilter
    {
        public int[] OptionsStatusRefund { get; private set; }
        public int[] OptionsStatusTranslate { get; private set; }
        public int[] OptionsStatusGPT { get; private set; }
        public FrontFilter(int[] optionsStatusRefund, int[] optionsStatusTranslate, int[] optionsStatusGPT)
        {
            OptionsStatusRefund = optionsStatusRefund;
            OptionsStatusTranslate = optionsStatusTranslate;
            OptionsStatusGPT = optionsStatusGPT;
        }
    }
}
