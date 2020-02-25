using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Views.InputMethods;
using Plugin.CurrentActivity;
using Wolf.Utility.Droid.Services;
using Wolf.Utility.Main.Logging;
using Wolf.Utility.Main.Logging.Enum;
using Wolf.Utility.Main.Xamarin.Services;

namespace Wolf.Utility.Droid.Services
{
    public class KeyboardService : IKeyboardService
    {
        private InputMethodManager inputMethodManager;
        private readonly object mainActivity;
        public KeyboardService(object activity, InputMethodManager methodManager)
        {
            mainActivity = activity;
            inputMethodManager = methodManager;
        }
        public bool IsKeyboardShown => inputMethodManager.IsAcceptingText;

        public void HideKeyboard()
        {
            if (!IsKeyboardShown) return;

            if (HideKeyboardAttemptOne()) return;
            
            if (HideKeyboardAttemptTwo()) return;

            if (HideKeyboardAttemptThree()) return;

            if (HideKeyboardAttemptFour()) return;
        }

        private bool HideKeyboardAttemptOne()
        {
            if (inputMethodManager == null || !(mainActivity is Activity activity)) return false;

            Logging.Log(LogType.Information, $"Attempting to Hide Keyboard via 1st method...");

            //var view = activity.CurrentFocus;
            var view = activity.FindViewById(Android.Resource.Id.Content).RootView;
            if (view == null) Logging.Log(LogType.Warning, $"Failed to get View from Activity...");

            var token = view?.WindowToken;
            if (token == null) Logging.Log(LogType.Warning, $"Failed to get Token from View...");

            var success = inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);
            Logging.Log(LogType.Information,
                $"{nameof(inputMethodManager.HideSoftInputFromWindow)} returned => {success}");

            if (success) view?.ClearFocus();
            if (!IsKeyboardShown)
            {
                view?.ClearFocus();
                return true;
            }

            Logging.Log(LogType.Warning,
                $"Failed to Hide Keyboard via {nameof(inputMethodManager.HideSoftInputFromWindow)} using standard ways of getting the view...");
            return false;
        }

        private bool HideKeyboardAttemptTwo()
        {
            if (inputMethodManager == null || !(mainActivity is Activity activity)) return false;

            Logging.Log(LogType.Information, $"Attempting to Hide Keyboard via 2nd method...");

            //var view = activity.CurrentFocus;
            var view = activity.FindViewById(Android.Resource.Id.Content).RootView;
            if (view == null) Logging.Log(LogType.Warning, $"Failed to get View from Activity...");

            var token = view?.WindowToken;
            if (token == null) Logging.Log(LogType.Warning, $"Failed to get Token from View...");

            inputMethodManager.ToggleSoftInputFromWindow(token, ShowSoftInputFlags.None, HideSoftInputFlags.None);
            
            if (!IsKeyboardShown)
            {
                view?.ClearFocus();
                return true;
            }

            Logging.Log(LogType.Warning, $"Failed to Hide Keyboard via {nameof(inputMethodManager.ToggleSoftInputFromWindow)}...");
            return false;
        }

        private bool HideKeyboardAttemptThree()
        {
            Logging.Log(LogType.Information, $"Attempting to Hide Keyboard via 3rd method...");

            var view = CrossCurrentActivity.Current.Activity.CurrentFocus;
            if (view == null) Logging.Log(LogType.Warning, $"Failed to get View from Window...");

            var token = view?.WindowToken;
            if (token == null) Logging.Log(LogType.Warning, $"Failed to get Token from View...");

            var success = inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);
            Logging.Log(LogType.Information,
                $"{nameof(inputMethodManager.HideSoftInputFromWindow)} returned => {success}");

            if (success) view?.ClearFocus();
            if (!IsKeyboardShown)
            {
                view?.ClearFocus();
                return true;
            }

            Logging.Log(LogType.Warning,
                $"Failed to Hide Keyboard via {nameof(inputMethodManager.HideSoftInputFromWindow)} using CrossCurrentActivity to get the view...");
            return false;
        }
        
        private bool HideKeyboardAttemptFour()
        {
            Logging.Log(LogType.Information, $"Attempting to Hide Keyboard via 4th method...");
            
            var context = CrossCurrentActivity.Current.AppContext;
            var imm = InputMethodManager.FromContext(context);

            var view = CrossCurrentActivity.Current.Activity.Window.DecorView;
            var token = view.WindowToken;

            var success = imm.HideSoftInputFromWindow(token, HideSoftInputFlags.NotAlways);
            Logging.Log(LogType.Information,
                $"{nameof(inputMethodManager.HideSoftInputFromWindow)} returned => {success}");

            var focus = CrossCurrentActivity.Current.Activity.CurrentFocus;

            if (success) focus?.ClearFocus();
            if (!IsKeyboardShown)
            {
                focus?.ClearFocus();
                return true;
            }
            
            Logging.Log(LogType.Warning,
                $"Failed to Hide Keyboard via {nameof(inputMethodManager.HideSoftInputFromWindow)} using CrossCurrentActivity to get the view, as an exact StackOverflow copy...");
            return false;
        }

        public void ReInitializeInputMethod()
        {
            inputMethodManager = InputMethodManager.FromContext((Context) mainActivity);
        }
    }
}