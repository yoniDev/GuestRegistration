using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Newtonsoft.Json;

namespace Vamk.Activities
{
    [Activity(Label = "Vamk Guest", Theme = "@style/AppTheme")]
    public class GuestProfileActivity : AppCompatActivity
    {
        private Toolbar mToolBar;
        private TextView mName, mEmail;
        private ImageView mProfilePic;
        private Button mNotice;
        private CollapsingToolbarLayout mCollapsingToolBar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.GuestProfile);

            mToolBar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            mCollapsingToolBar = FindViewById<CollapsingToolbarLayout>(Resource.Id.collapsing_toolbar);
            mName = FindViewById<TextView>(Resource.Id.txtname);
            mEmail = FindViewById<TextView>(Resource.Id.txtemail);
            mProfilePic = FindViewById<ImageView>(Resource.Id.picture);
            mNotice = FindViewById<Button>(Resource.Id.btnnotice);

            mNotice.Click += MNotice_Click;

            SetSupportActionBar(mToolBar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            mCollapsingToolBar.Title = "Vamk Guest";
        }

        private void MNotice_Click(object sender, System.EventArgs e)
        {
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }

        protected override void OnResume()
        {
            base.OnResume();

            var jsonGuest = Intent.GetStringExtra("guest");
            var Guest = JsonConvert.DeserializeObject<Models.Guest>(jsonGuest);

            mName.Text = Guest.LastName + ", " + Guest.FirstName;
            mEmail.Text = "Email: " + Guest.Email;
            mNotice.Text = "Registered Successfully!" + "\nThankyou for visiting VAMK!";

            int height = Resources.DisplayMetrics.HeightPixels;
            int width = mProfilePic.Height;

            if(App.bitmap != null)
            {
                App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);
                mProfilePic.SetImageBitmap(App.bitmap);
            }                       
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    StartActivity(new Intent(Application.Context, typeof(MainActivity)));
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}