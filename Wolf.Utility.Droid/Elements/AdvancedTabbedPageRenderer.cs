using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Wolf.Utility.Droid.Elements;
using Wolf.Utility.Main.Logging;
using Wolf.Utility.Main.Logging.Enum;
using Wolf.Utility.Main.Xamarin.Elements;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(AdvancedTabbedPage), typeof(AdvancedTabbedPageRenderer))]
namespace Wolf.Utility.Droid.Elements
{
    // Source: https://stackoverflow.com/questions/48658921/tabbedpage-hide-all-tabs

    public class AdvancedTabbedPageRenderer : TabbedPageRenderer
    {
        private TabLayout TabsLayout { get; set; }
        private ViewPager PagerLayout { get; set; }
        private AdvancedTabbedPage CurrentTabbedPage { get; set; }

        public AdvancedTabbedPageRenderer(Context context) : base(context)
        {
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
                CurrentTabbedPage = (AdvancedTabbedPage) e.NewElement;
            else
                CurrentTabbedPage = (AdvancedTabbedPage) e.OldElement;

            //find the pager and tabs
            for (int i = 0; i < ChildCount; ++i)
            {
                var view = (Android.Views.View)GetChildAt(i);
                if (view is TabLayout) TabsLayout = (TabLayout)view;
                else if (view is ViewPager) PagerLayout = (ViewPager)view;
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            TabsLayout.Visibility = ((AdvancedTabbedPage)Element).IsTabsHidden ? ViewStates.Gone : ViewStates.Visible;
        }
    }
}