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

        ListView listView;
        List<Entry> entries = new List<Entry>();
        ChecklistAdapter adapter;
        const string FILE_NAME = "entries.json";

        ImageButton fabAdd;
        CoordinatorLayout coordinatorLayout;

        bool doubleBackToExitPressedOnce = false;
        int maxLength = 35;

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

            listView = FindViewById<ListView>(Resource.Id.list);
            adapter = new ChecklistAdapter(this, entries);
            LoadList();

            listView.Adapter = adapter;
            RegisterForContextMenu(listView);

            fabAdd = FindViewById<ImageButton>(Resource.Id.add_fab);

            fabAdd.Click += (sender, e) =>
            {
                var alertAdd = new AppCompat.AlertDialog.Builder(this);
                alertAdd.SetMessage("Entry name (Limit " + maxLength + " characters)");
                EditText inputBox = new EditText(this);
                inputBox.SetMaxLines(1);
                inputBox.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(maxLength) });
                alertAdd.SetView(inputBox);
                alertAdd.SetPositiveButton("Add", delegate
                {
                    if (!string.IsNullOrEmpty(inputBox.Text))
                    {
                        Entry newEntry = new Entry() { Title = inputBox.Text, DateModified = DateTime.Now };
                        RunOnUiThread(() =>
                        {
                            adapter.AddEntry(newEntry);
                        });
                    }
                });
                alertAdd.SetNegativeButton("Cancel", delegate { });
                alertAdd.Show();
            };

            coordinatorLayout = FindViewById<CoordinatorLayout>(Resource.Id.fab_layout);
        }

        #endregion load

        #region activities

        /// <summary>
        /// Called when the app is closed
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            SaveList();
        }

        /// <summary>
        /// Called then the app is backgrounded
        /// </summary>
        protected override void OnStop()
        {
            base.OnStop();
            SaveList();
        }

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
            Snackbar.Make(coordinatorLayout, "Press BACK again to exit", Snackbar.LengthShort).Show();

            // Toast.MakeText(this, "Press BACK again to exit", ToastLength.Short).Show();

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
        /// Called when item from toolbar is selected
        /// </summary>
        /// <param name="item"></param>
        /// <returns>True if selected, false if not</returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var selectedItem = item.TitleFormatted.ToString();
            switch (selectedItem)
            {
                case "Clear":
                    if (adapter.AnyChecked())
                    {
                        var alertClear = new AppCompat.AlertDialog.Builder(this);
                        alertClear.SetMessage("Clear all completed items?");
                        alertClear.SetPositiveButton("Yes", delegate
                        {
                            RunOnUiThread(() =>
                            {
                                adapter.ClearEntries();
                            });
                        });
                        alertClear.SetNegativeButton("No", delegate { });
                        alertClear.Show();
                    }
                    else
                    {
                        Toast.MakeText(this, "No completed entries to remove", ToastLength.Short).Show();
                    }
                    break;
            }
            return base.OnOptionsItemSelected(item);
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

        /// <summary>
        /// Called when a context menu item is selected
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool OnContextItemSelected(IMenuItem item)
        {
            var info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
            var selectedTitle = item.TitleFormatted.ToString();

            switch (selectedTitle)
            {
                case "Edit":
                    var editDialog = new AppCompat.AlertDialog.Builder(this);
                    editDialog.SetTitle("Edit entry (Limit " + maxLength + " characters)");
                    EditText editBox = new EditText(this);
                    editBox.SetMaxLines(1);
                    editBox.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(maxLength) });
                    editBox.Text = entries[info.Position].Title;
                    editDialog.SetView(editBox);
                    editDialog.SetPositiveButton("Save", delegate
                    {
                        if (!string.IsNullOrEmpty(editBox.Text))
                        {
                            RunOnUiThread(() =>
                            {
                                adapter.EditEntry(info.Position, editBox.Text);
                            });
                        }
                    });
                    editDialog.SetNegativeButton("Cancel", delegate { });
                    editDialog.Show();
                    break;
                case "Delete":
                    var deleteDialog = new AppCompat.AlertDialog.Builder(this);
                    deleteDialog.SetTitle("Delete");
                    deleteDialog.SetMessage("Are you sure you want to delete this item?");
                    deleteDialog.SetPositiveButton("Yes", delegate
                    {
                        RunOnUiThread(() =>
                        {
                            adapter.DeleteEntry(info.Position);
                        });
                    });
                    deleteDialog.SetNegativeButton("No", delegate { });
                    deleteDialog.Show();
                    break;
            }
            return base.OnContextItemSelected(item);
        }

        #endregion activities

        #region methods

        /// <summary>
        /// Loads data for the list if it exists
        /// </summary>
        private void LoadList()
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, FILE_NAME);

            if (File.Exists(filePath))
            {
                var result = JsonSerialization<Entry>.DeserializeList(filePath);

                if (result.Success)
                {
                    List<Entry> entriesToLoad = result.Data;
                    if (entriesToLoad.Count > 0 && entriesToLoad != null)
                    {
                        foreach (Entry entry in entriesToLoad)
                        {
                            RunOnUiThread(() =>
                            {
                                adapter.AddEntry(entry);
                            });
                        }
                    }

                }
                else
                {
                    Console.WriteLine(result.Exception.Message);
                }

            }
        }

        /// <summary>
        /// Saves list data
        /// </summary>
        private void SaveList()
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, FILE_NAME);

            List<Entry> entriesToSave = adapter.GetEntries();

            var result = JsonSerialization<Entry>.SerializeList(filePath, entriesToSave);

            if (!result.Success)
            {
                Console.WriteLine(result.Exception.Message);
            }
        }

        #endregion methods
    }
}
