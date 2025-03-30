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
                MinimumWidth = 800,
                MinimumHeight = 500
            };
        }
    }
}