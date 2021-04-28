using System;
using System.Collections.Generic;

namespace Model
{
    public class RaceFinishedEventArgs : EventArgs
    {
        public Queue<IParticipant> FinishedParticipants { get; set; }
        public Queue<TimeSpan> FinishedTimeSpans { get; set; }
    }
}