using Foundation;
using UIKit;

namespace ToyIdentifier.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            LoadApplication(new App());
            UIApplication.SharedApplication.StatusBarHidden = false;

            return base.FinishedLaunching(app, options);
        }
    }
}
