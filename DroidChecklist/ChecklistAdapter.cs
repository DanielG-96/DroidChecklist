using System;
using System.Collections.Generic;
using UsefulStuffPortable.Extensions;

using Android.App;
using Android.Widget;
using Android.Views;

namespace DroidChecklist
{
    public class ChecklistAdapter : BaseAdapter<Entry>
    {
        #region vars

        List<Entry> _entries;
        Activity _activity;

        #endregion vars

        #region load

        /// <summary>
        /// Constructor for custom checklist adapter
        /// </summary>
        /// <param name="activity">Activity that adapter is part of</param>
        /// <param name="entries">List for checklist entries</param>
        public ChecklistAdapter(Activity activity, List<Entry> entries) : base()
        {
            _activity = activity;
            _entries = entries;
        }

        #endregion load

        #region adapter methods

        /// <summary>
        /// Gets the item ID from a specified list position
        /// </summary>
        /// <param name="position">Position number</param>
        /// <returns>Item ID</returns>
        public override long GetItemId(int position)
        {
            return position;
        }

        /// <summary>
        /// Gets a single entry from a specified list position
        /// </summary>
        /// <param name="position">Position number</param>
        /// <returns>List entry</returns>
        public override Entry this[int position]
        {
            get { return _entries[position]; }
        }

        /// <summary>
        /// Gets the list count
        /// </summary>
        public override int Count
        {
            get { return _entries.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder; // creates a new view holder for each list item to improve performance
            View view = convertView;

            if (view == null)
            {
                holder = new ViewHolder();
                view = _activity.LayoutInflater.Inflate(Resource.Layout.Checklist, null); // inflate the checklist layout on the view
                holder.Description = view.FindViewById<TextView>(Resource.Id.desc); // entry description
                holder.Date = view.FindViewById<TextView>(Resource.Id.date); // date of entry added/completion
                holder.CheckBox = view.FindViewById<CheckBox>(Resource.Id.checkbox); // checkbox for entry
                holder.CheckBox.Focusable = false; // prevents checkbox from stealing event from list item
                holder.CheckBox.FocusableInTouchMode = false; // likewise in touch mode
                holder.CheckBox.Clickable = true; // but ensure that it is clickable
                view.Tag = holder; // tag the view with the view holder
            }
            else
            {
                holder = (ViewHolder)view.Tag;
            }

            Entry entry = _entries[position]; // creates an entry at the specified view position

            if(entry != null)
            {
                holder.Description.Text = entry.Title; // Entry description

                // Converts the entry date to a readable time
                if (entry.Checked)
                    holder.Date.Text = "Completed " + entry.DateCompleted.ToPrettyDate();
                else
                    holder.Date.Text = "Added " + entry.DateModified.ToPrettyDate();

                holder.CheckBox.Checked = entry.Checked;
                holder.CheckBox.CheckedChange += (sender, e) => // checkbox event for when a user checks/unchecks an item
                {
                    entry.Checked = holder.CheckBox.Checked;
                    if(entry.Checked)
                    {
                        entry.DateCompleted = DateTime.Now;
                        holder.Date.Text = "Completed " + entry.DateCompleted.ToPrettyDate();
                    }
                    else
                    {
                        entry.DateCompleted = new DateTime();
                        holder.Date.Text = "Added " + entry.DateModified.ToPrettyDate();
                    }
                };

            }
            
            return view;
        }

        public class ViewHolder : Java.Lang.Object
        {
            public TextView Description { get; set; }
            public TextView Date { get; set; }
            public CheckBox CheckBox { get; set; }
        }

        #endregion adapter methods

        #region list methods

        /// <summary>
        /// Adds a new entry to the list
        /// </summary>
        /// <param name="entry">Entry model</param>
        public void AddEntry(Entry entry)
        {
            _entries.Add(entry);
            #if DEBUG
                Console.WriteLine("Added entry " + entry.Title + " at " + entry.DateModified);
            #endif
            NotifyDataSetChanged();
        }

        /// <summary>
        /// Edits the text of an entry at the specified position
        /// </summary>
        /// <param name="pos">Item position</param>
        /// <param name="text">Item text</param>
        public void EditEntry(int pos, string text)
        {
            _entries[pos].Title = text;
            NotifyDataSetChanged();
        }

        /// <summary>
        /// Removes an entry from the list at the specified position
        /// </summary>
        /// <param name="pos"></param>
        public void DeleteEntry(int pos)
        {
            Entry e = _entries[pos];
            _entries.Remove(e);
            NotifyDataSetChanged();
        }

        /// <summary>
        /// Removes the completed entries from the list. This is done by iterating the list in reverse to avoid null exceptions
        /// </summary>
        public void ClearEntries()
        {
            for(int i = _entries.Count - 1; i >= 0; i--)
            {
                if(_entries[i].Checked)
                {
                    Entry e = _entries[i];
                    _entries.Remove(e);
                    NotifyDataSetChanged();
                }
            }
        }

        /// <summary>
        /// Returns the list of entries from this adapter (pretty dirty way)
        /// </summary>
        /// <returns>List of entries</returns>
        public List<Entry> GetEntries()
        {
            return _entries;
        }

        /// <summary>
        /// Gets if any entries are checked at all or not
        /// </summary>
        /// <returns>True if at least one entry is checked, false otherwise</returns>
        public bool AnyChecked()
        {
            for(int i = _entries.Count - 1; i >= 0; i--)
            {
                if (_entries[i].Checked)
                    return true;
            }
            return false;
        }

        #endregion list methods
    }
}

