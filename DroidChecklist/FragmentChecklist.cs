using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Support.V7.App;
using Android.Text;

namespace DroidChecklist
{
    public class FragmentChecklist : Fragment
    {
        const string FILE_NAME = "entries.json";

        private Context mContext;
        private FloatingActionButton fabAdd;
        private CoordinatorLayout mLayout;
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private List<Entry> mEntriesList;
        private ChecklistRecyclerAdapter mAdapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mContext = Activity;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_checklist, container, false);
            
            mLayout = view.FindViewById<CoordinatorLayout>(Resource.Id.layout_coordinator);

            mRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.list_view_entries);
            mEntriesList = new List<Entry>();

            mLayoutManager = new LinearLayoutManager(mContext);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            mAdapter = new ChecklistRecyclerAdapter(Activity, mEntriesList);
            mRecyclerView.SetAdapter(mAdapter);

            LoadList();

            fabAdd = view.FindViewById<FloatingActionButton>(Resource.Id.fab_add);
            fabAdd.Click += (sender, e) =>
            {
                var alertAdd = new AlertDialog.Builder(mContext);
                alertAdd.SetMessage("Entry name");
                EditText inputBox = new EditText(mContext);
                inputBox.SetMaxLines(1);
                alertAdd.SetView(inputBox);
                alertAdd.SetPositiveButton("Add", delegate
                {
                    if (!string.IsNullOrEmpty(inputBox.Text))
                    {
                        Entry newEntry = new Entry() { Title = inputBox.Text, DateModified = DateTime.Now };
                        Activity.RunOnUiThread(() =>
                        {
                            mAdapter.AddEntry(newEntry);
                        });
                    }
                });
                alertAdd.SetNegativeButton("Cancel", delegate { });
                alertAdd.Show();
            };

            return view;
        }

        /// <summary>
        /// Called when the app is closed
        /// </summary>
        public override void OnDestroy()
        {
            base.OnDestroy();
            SaveList();
        }

        /// <summary>
        /// Called then the app is backgrounded
        /// </summary>
        public override void OnStop()
        {
            base.OnStop();
            SaveList();
        }

        /// <summary>
        /// Called when item from toolbar is selected
        /// </summary>
        /// <param name = "item" ></ param >
        /// < returns > True if selected, false if not</returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var selectedItem = item.TitleFormatted.ToString();
            switch (selectedItem)
            {
                case "Clear":
                    Toast.MakeText(mContext, "Not implemented!", ToastLength.Short).Show();
                    break;
            }
            return base.OnOptionsItemSelected(item);
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
                    Toast.MakeText(mContext, "Not implemented!", ToastLength.Short).Show();
                    //var editDialog = new AlertDialog.Builder(mContext);
                    //editDialog.SetTitle("Edit entry (Limit " + maxLength + " characters)");
                    //EditText editBox = new EditText(mContext);
                    //editBox.SetMaxLines(1);
                    //editBox.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(maxLength) });
                    //editBox.Text = entries[info.Position].Title;
                    //editDialog.SetView(editBox);
                    //editDialog.SetPositiveButton("Save", delegate
                    //{
                    //    if (!string.IsNullOrEmpty(editBox.Text))
                    //    {
                    //        Activity.RunOnUiThread(() =>
                    //        {
                    //            mAdapter.EditEntry(info.Position, editBox.Text);
                    //        });
                    //    }
                    //});
                    //editDialog.SetNegativeButton("Cancel", delegate { });
                    //editDialog.Show();
                    break;
                case "Delete":
                    Toast.MakeText(mContext, "Not implemented!", ToastLength.Short).Show();
                    //var deleteDialog = new AlertDialog.Builder(mContext);
                    //deleteDialog.SetTitle("Delete");
                    //deleteDialog.SetMessage("Are you sure you want to delete this item?");
                    //deleteDialog.SetPositiveButton("Yes", delegate
                    //{
                    //    Activity.RunOnUiThread(() =>
                    //    {
                    //        mAdapter.DeleteEntry(info.Position);
                    //    });
                    //});
                    //deleteDialog.SetNegativeButton("No", delegate { });
                    //deleteDialog.Show();
                    break;
            }
            return base.OnContextItemSelected(item);
        }

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
                    mEntriesList = result.Data;
                }
                else
                {
                    Console.Out.WriteLine(result.Exception.Message);
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
            
            var result = JsonSerialization<Entry>.SerializeList(filePath, mEntriesList);

            if (!result.Success)
            {
                Console.Out.WriteLine(result.Exception.Message);
            }
        }

        #endregion methods
    }
}