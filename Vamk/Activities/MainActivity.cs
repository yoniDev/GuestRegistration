using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vamk.Models;
using Vamk.Services;
using Environment = Android.OS.Environment;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Uri = Android.Net.Uri;

namespace Vamk.Activities
{
    [Activity(Label = "Vamk Guest", Icon = "@drawable/icon", Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity
    {
        private EditText txtFirstName, txtLastName, txtEmail;
        private Button BtnRegister;
        private Toolbar mToolBar;
        private LinearLayout mHostLayout;
        private ImageView ImageView;
        private CheckBox CheckedM, CheckedF;
        private Guest guest;
        private GuestServices gs;
        private ProgressDialog mProgressBar;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            gs = new GuestServices();
            guest = new Guest();
            mProgressBar = new ProgressDialog(this) { Indeterminate = true };
            mProgressBar.SetTitle("Vamk Guest");
            mProgressBar.SetMessage("Registering ...");

            mToolBar = FindViewById<Toolbar>(Resource.Id.toolBar);

            txtFirstName = FindViewById<EditText>(Resource.Id.txtFname);
            txtLastName = FindViewById<EditText>(Resource.Id.txtLname);
            txtEmail = FindViewById<EditText>(Resource.Id.txtEmail);

            CheckedM = FindViewById<CheckBox>(Resource.Id.malecheckbox);
            CheckedF = FindViewById<CheckBox>(Resource.Id.femalecheckbox);
            mHostLayout = FindViewById<LinearLayout>(Resource.Id.layout_main);
            BtnRegister = FindViewById<Button>(Resource.Id.btnregister);

            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();
                ImageView = FindViewById<ImageView>(Resource.Id.picture);
            }

            ImageView.Visibility = ViewStates.Gone;
            CheckedM.CheckedChange += CheckedM_CheckedChange;
            CheckedF.CheckedChange += CheckedF_CheckedChange;

            BtnRegister.Click += BtnRegister_Click;
            SetSupportActionBar(mToolBar);

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_takepic:
                    TakePic();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void CheckedF_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked)
            {
                guest.Sex = "Female";
            }
        }

        private void CheckedM_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked)
            {
                guest.Sex = "Male";
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // Make it available in the gallery

            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Uri contentUri = Uri.FromFile(App._file);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);

            // Display in ImageView. We will resize the bitmap to fit the display
            // Loading the full sized image will consume to much memory 
            // and cause the application to crash.
            ImageView.Visibility = ViewStates.Visible;
            int height = Resources.DisplayMetrics.HeightPixels;
            int width = ImageView.Height;
            App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);
            if (App.bitmap != null)
            {
                ImageView.SetImageBitmap(App.bitmap);

                //using (var stream = new MemoryStream())
                //{
                //    App.bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                //    guest.Image = stream.ToArray();
                //}
                App.bitmap = null;
            }

            // Dispose of the Java side bitmap.
            GC.Collect();
        }

        private void TakePic()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            App._file = new Java.IO.File(App._dir, String.Format("myPhoto_{0}.png", Guid.NewGuid()));
            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(App._file));
            StartActivityForResult(intent, 0);
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            guest.FirstName = txtFirstName.Text;
            guest.LastName = txtLastName.Text;
            guest.Email = txtEmail.Text;

            if (string.IsNullOrEmpty(guest.FirstName) || string.IsNullOrEmpty(guest.LastName))
            {
                Snackbar.Make(mHostLayout, "Please fill in missing fields!", Snackbar.LengthLong)
                       .Show();

            }
            else
            {
                RegisterGuest();
            }

        }

        private void RegisterGuest()
        {
            mProgressBar.Show();
            new Java.Lang.Thread(async () =>
            {
                await gs.SaveUser(guest);
                RunOnUiThread(() => onSuccessfulRegistration());
            }).Start();
        }

        private void onSuccessfulRegistration()
        {
            mProgressBar.Hide();
            StartActivity(new Intent(this, typeof(GuestProfileActivity))
                .PutExtra("guest", JsonConvert.SerializeObject(guest)));
        }

        private void CreateDirectoryForPictures()
        {
            App._dir = new Java.IO.File(
                Environment.GetExternalStoragePublicDirectory(
                Environment.DirectoryPictures), "CameraVamkGuest");
            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
            }
        }

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

    }

    public static class App
    {
        public static Java.IO.File _file;
        public static Java.IO.File _dir;
        public static Bitmap bitmap;
    }


}

