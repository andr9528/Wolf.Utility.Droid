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

namespace Wolf.Utility.Droid.Extensions.Methods
{
    public static class ContextExtensions
    {
        public static ComponentName StartService<T>(this Context context) where T : Service
        {
            var intent = new Intent(context, typeof(T));
            return context.StartService(intent);
        }
    }
}