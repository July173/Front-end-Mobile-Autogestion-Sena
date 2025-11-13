using Android.App;
using Android.Runtime;
using Experiencias_Significativas_App.MAUI;

namespace Experiencias_Significativas_App.MAUI;

[Application]
public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
