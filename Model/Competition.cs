using System.Collections.Generic;

namespace Model
{
    public class Competition
    {
        public List<IParticipant> Participants { get; set; }
        public Queue<Track> Tracks { get; set; }

        public DataContainer<ParticipantRaceScore> ParticipantScoreDataContainer { get; set; }
        public DataContainer<ParticipantRaceTime> ParticipantRaceTimeDataContainer { get; set; }
        public DataContainer<ParticipantRoundTime> ParticipantRoundTimeDataContainer { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Competition()
        {
            Participants = new List<IParticipant>();
            Tracks = new Queue<Track>();

            ParticipantScoreDataContainer = new DataContainer<ParticipantRaceScore>();
            ParticipantRaceTimeDataContainer = new DataContainer<ParticipantRaceTime>();
            ParticipantRoundTimeDataContainer = new DataContainer<ParticipantRoundTime>();
        }

        /// <summary>
        /// Sets the next track by dequeueing
        /// </summary>
        /// <returns></returns>
        public Track NextTrack()
        {
            return Tracks.TryDequeue(out var track) ? track : null;
        }

    }
}