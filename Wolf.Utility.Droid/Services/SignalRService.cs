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
using Microsoft.AspNetCore.SignalR.Client;
using Wolf.Utility.Main.SignalR;

namespace Wolf.Utility.Droid.Services
{
    public abstract class SignalRService<T> : NotificationService where T : HubProxy
    {
        protected readonly string SignalRChannelId = $"{nameof(T)} Notifications";
        public T HubProxy { get; protected set; }
        protected bool ReconnectOnResume { get; set; }
        protected int ReconnectAttempts { get; set; }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if(ReconnectOnResume && HubProxy.ConnectionState == HubConnectionState.Disconnected) 
                HubProxy.Reconnect(ReconnectAttempts);

            return StartCommandResult.Sticky;
        }
    }
}   