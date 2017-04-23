using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using AppCompat = Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace DroidChecklist
{
    [Activity(MainLauncher = true, Label = "To-Do List", Icon = "@mipmap/ic_launcher", WindowSoftInputMode = SoftInput.AdjustPan)]
    public class MainActivity : AppCompatActivity
    {
        #region vars

        bool doubleBackToExitPressedOnce = false;

        #endregion vars;

        #region load

        /// <summary>
        /// Called when the app is initially created
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            // allows the status bar colour to be set for 5.0+
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Window.ClearFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            }

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "To-Do List";

            var fragment = new FragmentChecklist();
            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment).Commit();
        }

        #endregion load

        #region activities

        /// <summary>
        /// Called when the back button is pressed
        /// </summary>
        public override void OnBackPressed()
        {
            if (doubleBackToExitPressedOnce)
            {
                base.OnBackPressed();
                return;
            }

            doubleBackToExitPressedOnce = true;

            Toast.MakeText(this, "Press BACK again to exit", ToastLength.Short).Show();

            new Handler().PostDelayed(delegate
            {
                doubleBackToExitPressedOnce = false;
            },
            2000);
        }

        /// <summary>
        /// Called when toolbar options menu is created
        /// </summary>
        /// <param name="menu"></param>
        /// <returns>True if created, false if not</returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        /// <summary>
        /// Called when the context menu opens
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="v"></param>
        /// <param name="menuInfo"></param>
        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            menu.SetHeaderTitle("Select Action");
            menu.Add("Edit");
            menu.Add("Delete");
        }

        #endregion activities
    }
}
