using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace SimpleUrhoRoomScene.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			LaunchGame();
			return true;
		}

		async void LaunchGame()
		{
			await Task.Yield();
			new MyGame().Run();
		}
	}
}


