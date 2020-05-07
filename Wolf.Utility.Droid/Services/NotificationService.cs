using Android.App;
using Android.Content;
using Android.OS;

namespace Wolf.Utility.Droid.Services
{
    public abstract class NotificationService : Service
    {
        protected Context Context { get; set; }
        protected NotificationManager manager = null;

        protected void CreateNotificationChannel(string channelId, string channelName)
        {
            manager = NotificationManager.FromContext(Context);

            if (Build.VERSION.SdkInt < BuildVersionCodes.O) return;
            var channel = new NotificationChannel(channelId, channelName, NotificationImportance.Default)
            {
                Description = $"Notifications caused by a SignalR Hub"
            };
            manager.CreateNotificationChannel(channel);
        }

        /// <summary>
        /// Displays a notification, which does nothing when clicked.
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="autoCancel"></param>
        protected void ShowNotification(string channelId, string title, string text, bool autoCancel = true)
        {
            var builder = new Notification.Builder(Context, channelId)
                .SetContentTitle(title).SetContentText(text).SetAutoCancel(autoCancel);

            manager.Notify(1, builder.Build());
        }

        /// <summary>
        /// Displays a notification, which opens the specified activity when clicked.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channelId"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="autoCancel"></param>
        protected void ShowNotification<T>(string channelId, string title, string text, bool autoCancel = true) where T : Activity
        {
            var builder = new Notification.Builder(Context, channelId)
                .SetContentTitle(title).SetContentText(text).SetAutoCancel(autoCancel);

            var intent = new Intent(Context, typeof(T));
            var pending = PendingIntent.GetActivity(Context, 0, intent, PendingIntentFlags.UpdateCurrent);
            builder.SetContentIntent(pending);

            manager.Notify(1, builder.Build());
        }
    }
}