using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Nfc;
using Android.Nfc.Tech;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Ultz.XNFC;
using Wolf.Utility.Main.Logging;
using Wolf.Utility.Main.Logging.Enum;
using Wolf.Utility.Main.Nfc.Core;
using Wolf.Utility.Main.Nfc.Core.Enum;

namespace Wolf.Utility.Droid.Nfc.Implementation
{
    public class AndroidNfc : INfc
    {
        private NfcAdapter _adapter;

        public bool Scanning { get; private set; }

        public event EventHandler<TagDetectedEventArgs> TagDetected;

        public bool Enabled => _adapter.IsEnabled;

        public bool Available
        {
            get
            {
                if (Application.Context.CheckCallingOrSelfPermission("android.permission.NFC") == Permission.Granted)
                    return _adapter != null;
                return false;
            }
        }

        public object Association { get; set; }

        public bool CanScan => Available && Enabled;

        public AndroidNfc(object context)
        {
            _adapter = NfcAdapter.GetDefaultAdapter((Context)context);
            Association = context;
        }

        #region Start Tasks
        public Task StartListeningAsync(IEnumerable<ActionType> actions, IEnumerable<TechType> techs)
        {
            if (!Enabled)
                throw new InvalidOperationException("NFC not available");
            if (!Available)
                throw new InvalidOperationException("NFC is not enabled");

            List<string> filterActions = actions != null ? GetActions(actions) : null;
            List<string> filterTechs = techs != null ? GetTechs(techs) : null;

            Activity association = (Activity)Association;
            Intent intent = new Intent((Context)association, association.GetType()).AddFlags(ActivityFlags.SingleTop);
            PendingIntent activity = PendingIntent.GetActivity((Context)association, 0, intent, (PendingIntentFlags)0);

            var result = filterActions != null && filterTechs != null
                ? StartWithFilters(filterActions, filterTechs, activity, association)
                : StartWithoutFilters(activity, association);

            Scanning = true;

            return result;
        }

        private Task StartWithoutFilters(PendingIntent activity, Activity association)
        {
            _adapter.EnableForegroundDispatch(association, activity, null, null);
            return Task.CompletedTask;
        }

        private Task StartWithFilters(IEnumerable<string> filterActions, IEnumerable<string> filterTechs, PendingIntent activity, Activity association)
        {
            IntentFilter intentFilter = new IntentFilter();
            foreach (var action in filterActions)
            {
                intentFilter.AddAction(action);
            }
            intentFilter.AddDataType("*/*");
            IntentFilter[] filters = new IntentFilter[1] { intentFilter };

            _adapter.EnableForegroundDispatch(association, activity, filters, new string[1][]
            {
                filterTechs.ToArray()
            });

            return Task.CompletedTask;
        }

        public async Task<bool> TryStartListeningAsync(IEnumerable<ActionType> actions, IEnumerable<TechType> techs)
        {
            try
            {
                if (CanScan)
                    await StartListeningAsync(actions, techs);
            }
            catch (System.Exception ex)
            {
                Logging.Log(LogType.Exception, $"{ex.GetType()} => {ex.Message}; Stacktrace => {ex.StackTrace}");
                return false;
            }
            return true;
        }
        #endregion

        #region Stop Tasks
        public Task StopListeningAsync()
        {
            _adapter?.DisableForegroundDispatch((Activity)Association);
            Scanning = false;

            return Task.CompletedTask;
        }

        public async Task<bool> TryStopListeningAsync()
        {
            try
            {
                if (Scanning)
                    await this.StopListeningAsync();
            }
            catch (System.Exception ex)
            {
                Logging.Log(LogType.Exception, $"{ex.GetType()} => {ex.Message}; Stacktrace => {ex.StackTrace}");
                return false;
            }
            return true;
        }
        #endregion

        public void Callback(object obj)
        {
            byte[] SELECT = {
                (byte) 0x00, // CLA Class           
                (byte) 0xA4, // INS Instruction     
                (byte) 0x04, // P1  Parameter 1
                (byte) 0x00, // P2  Parameter 2
                (byte) 0x0A, // Length
                0x63,0x64,0x63,0x00,0x00,0x00,0x00,0x32,0x32,0x31 // AID
            };

            byte[] GET_STRING = {
                (byte) 0x80, // CLA Class        
                0x04, // INS Instruction
                0x00, // P1  Parameter 1
                0x00, // P2  Parameter 2
                0x10  // LE  maximal number of bytes expected in result
            };

            if (obj is Intent intent)
            {
                Logging.Log(LogType.Information, $"Callback object is of type 'Intent'");

                Tag rawTag = intent.GetParcelableExtra(NfcAdapter.ExtraTag) as Tag;

                IsoDep tag = IsoDep.Get(rawTag);
                
                tag.Connect();
                byte[] result = tag.Transceive(SELECT);

                if (!(result[0] == (byte)0x90 && result[1] == (byte)0x00))
                    throw new IOException("could not select applet");

                result = tag.Transceive(GET_STRING);
                int len = result.Length;

                if (!(result[len - 2] == (byte)0x90 && result[len - 1] == (byte)0x00))
                    throw new RuntimeException("could not retrieve msisdn");

                byte[] data = new byte[len - 2];

                Array.Copy(result, data, len - 2);

                var str = Encoding.Default.GetString(data).Trim();

                tag.Close();
            }
        }

        private List<string> GetActions(IEnumerable<ActionType> actions)
        {
            var output = new List<string>();

            foreach (var action in actions.Distinct())
            {
                switch (action)
                {
                    case ActionType.ActionNdefDiscovered:
                        output.Add(NfcAdapter.ActionNdefDiscovered);
                        break;
                    case ActionType.ActionTechDiscovered:
                        output.Add(NfcAdapter.ActionTechDiscovered);
                        break;
                    case ActionType.ActionTagDiscovered:
                        output.Add(NfcAdapter.ActionTagDiscovered);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return output;
        }

        private List<string> GetTechs(IEnumerable<TechType> techs)
        {
            var output = new List<string>();

            foreach (var tech in techs.Distinct())
            {
                output.Add(tech.ToString());
            }

            return output;
        }
    }
}