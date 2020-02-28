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
using Wolf.Utility.Droid.Elements;
using Wolf.Utility.Xamarin.Elements;
using Xamarin.Forms;
using XLabs;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(AdvancedWebView), typeof(AdvancedWebViewRenderer))]
namespace Wolf.Utility.Droid.Elements
{
    public class AdvancedWebViewRenderer : HybridWebViewRenderer
    {
        public AdvancedWebViewRenderer()
        {

        }

        protected override Client GetWebViewClient()
        {
            Func<HybridWebViewRenderer, Client> viewClientDelegate = GetWebViewClientDelegate;
            if (viewClientDelegate == null)
                return new AdvancedWebViewRendererClient(this);
            return viewClientDelegate(this);
        }

        internal void OnPageStarted(string url)
        {
            Element?.Navigating(this, new EventArgs<Uri>(new Uri(url)));
        }
    }
}