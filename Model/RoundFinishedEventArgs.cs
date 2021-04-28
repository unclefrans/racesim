using System;

namespace Model
{
    public class RoundFinishedEventArgs : EventArgs
    {
        public int LapCount { get; set; }
        public string Name { get; set; }
        public TimeSpan Time { get; set; }
    }
}
