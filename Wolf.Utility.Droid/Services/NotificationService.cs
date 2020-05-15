using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Wolf.Utility.Main.Logging;
using Wolf.Utility.Main.Logging.Enum;

namespace Wolf.Utility.Droid.Services
{
    public abstract class NotificationService : Service
    {
        protected Context Context { get; set; }
        protected NotificationManager manager = null;
        protected bool IsStarted;

        protected void CreateNotificationChannel(string channelId, string channelName, string description)
        {
            if (Context == null) Logging.Log(LogType.Warning, $"The Context of this service is Null!");
            manager = NotificationManager.FromContext(Context);

            if (Build.VERSION.SdkInt < BuildVersionCodes.O) return;
            var channel = new NotificationChannel(channelId, channelName, NotificationImportance.Default)
            {
                Description = description
            };
            manager.CreateNotificationChannel(channel);
        }

        /// <summary>
        /// Displays a notification, which does nothing when clicked.
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="iconResourceId"></param>
        /// <param name="bigText"></param>
        /// <param name="priority"></param>
        /// <param name="autoCancel"></param>
        protected void ShowNotification(string channelId, string title, string text, int iconResourceId, string bigText = default, NotificationImportance priority = NotificationImportance.Default, bool autoCancel = true)
        {
            try
            {
                var builder = new NotificationCompat.Builder(Context, channelId)
                       .SetContentTitle(title).SetContentText(text).SetAutoCancel(autoCancel).SetPriority((int)priority).SetSmallIcon(iconResourceId);

                if (!string.IsNullOrEmpty(bigText))
                    using (var style = new NotificationCompat.BigTextStyle()) { builder.SetStyle(style.BigText(bigText)); }

                Logging.Log(LogType.Event, $"Firing Notification; Title: {title}; Text: {text}");
                manager.Notify(1, builder.Build());
            }
            catch (System.Exception e)
            {
                Logging.Log(LogType.Exception,
                    $"Something went wrong processing the new notification. Exception message: {e.Message}; Exception Stacktrace: {e.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// Displays a notification, which opens the specified activity when clicked.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channelId"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="iconResourceId"></param>
        /// <param name="bigText"></param>
        /// <param name="priority"></param>
        /// <param name="autoCancel"></param>
        protected void ShowNotification<T>(string channelId, string title, string text, int iconResourceId, string bigText = default, NotificationImportance priority = NotificationImportance.Default, bool autoCancel = true) where T : Activity
        {
            try
            {
                var builder = new NotificationCompat.Builder(Context, channelId)
                        .SetContentTitle(title).SetContentText(text).SetAutoCancel(autoCancel).SetPriority((int)priority).SetSmallIcon(iconResourceId);

                if (!string.IsNullOrEmpty(bigText))
                    using (var style = new NotificationCompat.BigTextStyle()) { builder.SetStyle(style.BigText(bigText)); }

                var intent = new Intent(Context, typeof(T));
                var pending = PendingIntent.GetActivity(Context, 0, intent, PendingIntentFlags.UpdateCurrent);
                builder.SetContentIntent(pending);

                Logging.Log(LogType.Event, $"Firing Notification with Intent; Title: {title}; Text: {text}");
                manager.Notify(1, builder.Build());
            }
            catch (System.Exception e)
            {
                Logging.Log(LogType.Exception,
                    $"Something went wrong processing the new notification. Exception message: {e.Message}; Exception Stacktrace: {e.StackTrace}");
                throw;
            }
        }
    }
}