
namespace SmartRefund.ViewModels.Requests
{
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
        public FrontFilter()
        {
            OptionsStatusRefund = new int[0];
            OptionsStatusTranslate = new int[0];
            OptionsStatusGPT = new int[0];
        }
    }
}
