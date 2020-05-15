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
using Wolf.Utility.Droid.Services;
using Wolf.Utility.Xamarin.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(CloseApplicationService))]
namespace Wolf.Utility.Droid.Services
{
    public class CloseApplicationService : ICloseApplicationService
    {
        public void CloseApplication()
        {
            Process.KillProcess(Process.MyPid());
        }
    }
}