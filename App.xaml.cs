namespace MattTools;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new MainPage();
	}
    public static Window Window { get; private set; }
    protected override Window CreateWindow(IActivationState activationState)
    {
        Window = base.CreateWindow(activationState);

        Window.Width = 1280;
        Window.Height = 832;
        Window.Page = MainPage;
        Window.Title = "Matt's Tools";

        return Window;
    }
}
