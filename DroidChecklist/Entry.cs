using System;

namespace DroidChecklist
{
    public class Entry
    {
        public string Title { get; set; }
        public string Notes { get; set; }
        public bool Checked { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCompleted { get; set; }
    }
}