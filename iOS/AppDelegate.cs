using Foundation;
using UIKit;
using Xamarin.Forms;

namespace ToyIdentifier.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            Forms.Init();

            LoadApplication(new App());
            UIApplication.SharedApplication.StatusBarHidden = false;

            DependencyService.Get<IImageClassifier>().Init("ToyIdentifier");

            return base.FinishedLaunching(uiApplication, launchOptions);
        }
    }
}
