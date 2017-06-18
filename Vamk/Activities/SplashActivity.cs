
using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Android.Widget;
using System.Threading.Tasks;

namespace Vamk.Activities
{
    [Activity(Theme = "@style/WorkforceTheme.Base", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.SplashLayout);
            base.OnCreate(savedInstanceState);
            Log.Debug(TAG, "SplashActivity.OnCreate");
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
            
        }

        // Simulates background work that happens behind the splash screen
        async void SimulateStartup()
        {
            Log.Debug(TAG, "Performing some startup work that takes a bit of time.");
            await Task.Delay(8000); // Simulate a bit of startup work.
            TextView companyName = FindViewById<TextView>(Resource.Id.splashtext);
            //companyName.TextSize = 18;

            //animator = ValueAnimator.OfInt(18, 44);
            //animator.Update += (object sender, ValueAnimator.AnimatorUpdateEventArgs e) =>
            //{
            //    int newValue = (int)e.Animation.AnimatedValue;
            //    companyName.TextSize = newValue;
            //};
            Finish();
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            
        }
    }
}