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
using Wolf.Utility.Main.SignalR;

namespace Wolf.Utility.Droid.Services
{
    public abstract class SignalRService<T> : NotificationService where T : HubProxy
    {
        protected readonly string SignalRChannelId = $"{nameof(T)} Notifications";
        public T HubProxy { get; protected set; }
    }
}   