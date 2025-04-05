namespace DiskSpaceAnalyzerMauiApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell())
            {
                MinimumWidth = 1000,
                MinimumHeight = 600
            };
        }
    }
}