using System;
using System.Collections.Generic;

namespace DroidChecklist
{
    public class Entry
    {
        public string Title { get; set; }
        public string Notes { get; set; }
        public bool Checked { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCompleted { get; set; }

        public static List<Entry> GetTestData()
        {
            List<Entry> li = new List<Entry>
            {
                new Entry() { Title = "Foo", Checked = false, Notes = "Bar" },
                new Entry() { Title = "Foo", Checked = true, Notes = "Bar" },
                new Entry() { Title = "Foo", Checked = true, Notes = "Bar" },
                new Entry() { Title = "Foo", Checked = false, Notes = "Bar" },
                new Entry() { Title = "Foo", Checked = true, Notes = "Bar" },
                new Entry() { Title = "Foo", Checked = true, Notes = "Bar" },
                new Entry() { Title = "Foo", Checked = true, Notes = "Bar" },
                new Entry() { Title = "Foo", Checked = false, Notes = "Bar" },
                new Entry() { Title = "Foo", Checked = true, Notes = "Bar" },
                new Entry() { Title = "Foo", Checked = true, Notes = "Bar" },
                new Entry() { Title = "Foo", Checked = true, Notes = "Bar" },
                new Entry() { Title = "Foo", Checked = false, Notes = "Bar" },
                new Entry() { Title = "Foo", Checked = true, Notes = "Bar" },
                new Entry() { Title = "Foo", Checked = true, Notes = "Bar" },
                new Entry() { Title = "Foo", Checked = false, Notes = "Bar" },
                new Entry() { Title = "Foo", Checked = false, Notes = "Bar" },
                new Entry() { Title = "Foo", Checked = false, Notes = "Bar" }
            };
            return li;
        }
    }
}