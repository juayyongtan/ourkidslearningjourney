using System.Timers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Distribute;
using System;
using System.Threading.Tasks;
using Android.Widget;

namespace PioneerCareSG
{
    [Activity(Label = "PioneerCare@SG", MainLauncher = true, Icon = "@mipmap/icon")]
    public class ActivitySplashScreen : Activity
    {
        private const long SPLASH_DELAY = 1000;
        private int counter = 0;
        private Timer timer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
			// Disable Mobile Center Distribute at runtime
			Distribute.SetEnabledAsync(true);
            Distribute.ReleaseAvailable = OnReleaseAvailable;
            MobileCenter.Start("f8da512a-363f-4c70-9505-b639f0a94f1a", typeof(Distribute));
            
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GUIActivity_SplashScreen);

            timer = new Timer();
            timer.Interval = SPLASH_DELAY;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (counter==3){
                timer.Stop();

                Intent intent = new Intent().SetClass(this,
                                                      typeof(MainActivity));
                
                Bundle bundle = ActivityOptionsCompat.MakeCustomAnimation(this,
                                                                          Resource.Transition.transition_pull_in_fromright,
                                                                          Resource.Transition.transition_push_out_toleft).ToBundle();
                StartActivity(intent, bundle);
                Finish();
            } else {
                counter++;
            }
        }

		private bool OnReleaseAvailable(ReleaseDetails releaseDetails)
		{
			// Look at releaseDetails public properties to get version information,
			// release notes text or release notes URL
			string versionName = releaseDetails.ShortVersion;
			string verionCodeOrBuildNumber = releaseDetails.Version;
			string releaseNotes = releaseDetails.ReleaseNotes;
			Uri releaseNotesUrl = releaseDetails.ReleaseNotesUrl;

            // Build dialog
            AlertDialog.Builder dialogBuilder = new AlertDialog.Builder(this);
            AlertDialog alert = dialogBuilder.Create();
            alert.SetTitle("Version: " + versionName + " is available!");
            alert.SetMessage(verionCodeOrBuildNumber);
            alert.SetButton("Ok",(sender, e) => {
                Distribute.NotifyUpdateAction(UpdateAction.Update);

                if (!releaseDetails.MandatoryUpdate) {
                    alert.SetButton2("Cancel",(sender1, e1) => {
                        Distribute.NotifyUpdateAction(UpdateAction.Postpone);
                    });
                } 
            });
            alert.SetCancelable(false);
            alert.Show();

			// Return true if you are using your own dialog
			// Return false otherwise
			return true;
		}
    }
}
