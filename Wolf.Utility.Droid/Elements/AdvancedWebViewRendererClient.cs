using Android.Graphics;
using Android.Webkit;
using XLabs.Forms.Controls;

namespace Wolf.Utility.Droid.Elements
{
    public class AdvancedWebViewRendererClient : HybridWebViewRenderer.Client
    {
        public AdvancedWebViewRendererClient(HybridWebViewRenderer webHybrid) : base(webHybrid)
        {
            
        }

        public override void OnPageStarted(WebView view, string url, Bitmap favicon)
        {
            base.OnPageStarted(view, url, favicon);

            if (WebHybrid == null || !WebHybrid.TryGetTarget(out var target))
                return;

            ((AdvancedWebViewRenderer) target).OnPageStarted(url);
        }

        public override void OnPageFinished(WebView view, string url)
        {
            base.OnPageFinished(view, url);
        }
    }
}