using MaterialDesignThemes.Wpf;

namespace MaterialDesignWithLiveChartSample.Model
{
    public class MainWindowModel
    {
        public PaletteHelper? MainThemePallette { get; set; }
        //public bool DarkTheme { get; set; } = true;
        public MainWindowModel()
        {
            MainThemePallette = new PaletteHelper();
        }
    }
}
