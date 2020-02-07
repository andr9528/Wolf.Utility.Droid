using Android.App;
using Android.Content;
using Android.Views.InputMethods;
using Wolf.Utility.Droid.Services;
using Wolf.Utility.Main.Xamarin.Services;
using Xamarin.Forms;

namespace Wolf.Utility.Droid.Services
{
    public class KeyboardService : IKeyboardService
    {
        private readonly InputMethodManager inputMethodManager;
        private readonly object mainActivity;
        public KeyboardService(object activity, InputMethodManager methodManager)
        {
            mainActivity = activity;
            inputMethodManager = methodManager;
        }
        public bool IsKeyboardShown => inputMethodManager.IsAcceptingText;

        public void HideKeyboard()
        {
            if (inputMethodManager != null && mainActivity is Activity)
            {
                var activity = mainActivity as Activity;
                var token = activity?.CurrentFocus?.WindowToken;
                inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);

                activity?.Window.DecorView.ClearFocus();
            }
        }
    }
}