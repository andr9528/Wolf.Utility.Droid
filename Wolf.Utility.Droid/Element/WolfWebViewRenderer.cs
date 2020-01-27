using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Wolf.Utility.Droid.Element;
using Wolf.Utility.Main.Logging;
using Wolf.Utility.Main.Logging.Enum;
using Wolf.Utility.Main.Xamarin.Elements;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(WolfWebView), typeof(WolfWebViewRenderer))]
namespace Wolf.Utility.Droid.Element
{
    public class WolfWebViewRenderer : WebViewRenderer
    {
        public WolfWebViewRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            Logging.Log(LogType.Event, $"WolfWebViewRenderer OnElementChanged Called");
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetWebViewClient(new WolfWebviewClient());

                Control.Touch += (sender, eventArgs) => {

                    Logging.Log(LogType.Event, $"WolfWebViewRenderer Touch Called");
                    if (eventArgs.Event.Action == MotionEventActions.Down)
                    {
                        Logging.Log(LogType.Event, $"With MotionEventActions.Down");

                        MessagingCenter.Send<object>(this, "webviewClicked");
                    }

                    if (eventArgs.Event.Action == MotionEventActions.Up)
                    {
                        Logging.Log(LogType.Event, $"With MotionEventActions.Up");

                        MessagingCenter.Send<object>(this, "webviewClicked");
                    }

                    if (eventArgs.Event.Action == MotionEventActions.ButtonPress)
                    {
                        Logging.Log(LogType.Event, $"With MotionEventActions.ButtonPress");

                        MessagingCenter.Send<object>(this, "webviewClicked");
                    }
                };

            }

        }
    }
}