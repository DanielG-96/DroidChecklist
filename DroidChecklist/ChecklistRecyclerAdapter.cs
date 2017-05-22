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
using Android.Support.V7.Widget;

namespace DroidChecklist
{
    public class ChecklistRecyclerAdapter : RecyclerView.Adapter
    {
        private List<Entry> mEntryList;
        private Context mContext;
        public EventHandler<int> OnLongClick;

        public ChecklistRecyclerAdapter(Context context, List<Entry> entryList)
        {
            mContext = context;
            mEntryList = entryList;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.checklist_item, parent, false);
            
            return new ChecklistViewHolder(itemView);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            Entry entry = mEntryList[position];

            ChecklistViewHolder vh = (ChecklistViewHolder)holder;

            vh.TextTitle.Text = entry.Title;
            vh.TextNotes.Text = entry.Notes;
        }

        public override int ItemCount => mEntryList.Count;

        public void AddEntry(Entry entry)
        {
            mEntryList.Add(entry);
            NotifyItemInserted(mEntryList.Count - 1);
        }
    }

    public class ChecklistViewHolder : RecyclerView.ViewHolder
    {
        public TextView TextTitle { get; set; }
        public TextView TextNotes { get; set; }
        public CheckBox CheckCompletion { get; set; }

        public ChecklistViewHolder(View itemView) : base(itemView)
        {
            itemView.LongClick += (o, s) =>
            {
                Android.Support.V7.Widget.PopupMenu menu = new Android.Support.V7.Widget.PopupMenu(itemView.Context, itemView);
                menu.Inflate(Resource.Menu.context_menu);
                menu.Show();

                menu.MenuItemClick += (obj, sender) =>
                {
                    int item = sender.Item.ItemId;
                    switch (item)
                    {
                        case (Resource.Id.delete):
                            Toast.MakeText(itemView.Context, "Selected delete", ToastLength.Short).Show();
                            break;
                    }
                };
            };

            itemView.Click += (o, s) =>
            {
                Toast.MakeText(itemView.Context, "Click success", ToastLength.Short).Show();
            };

            TextTitle = itemView.FindViewById<TextView>(Resource.Id.text_title);
            TextNotes = itemView.FindViewById<TextView>(Resource.Id.text_notes);
            CheckCompletion = itemView.FindViewById<CheckBox>(Resource.Id.checkbox);
        }
    }
}