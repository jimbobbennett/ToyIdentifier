using Foundation;
using UIKit;
using Xam.Plugins.OnDeviceCustomVision;
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

            CrossImageClassifier.Current.Init("toyId", ModelType.General);

            return base.FinishedLaunching(uiApplication, launchOptions);
        }
    }
}
