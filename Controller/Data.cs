using Model;

namespace Controller
{
    public static class Data
    {
        public static Competition Competition { get; set; } = new Competition();
        public static Race CurrentRace { get; set; }

        public static void Initialize()
        {
            AddParticipants();
            AddTracks();
            NextRace();
        }

        /// <summary>
        /// Adds participants to the competition
        /// </summary>
        public static void AddParticipants() 
        {
            Competition.Participants.Add(new Driver("Aakje", 0, new Car(0, 0, 10, false), IParticipant.TeamColors.Blue));
            Competition.Participants.Add(new Driver("Bert", 0, new Car(0, 0, 10, false), IParticipant.TeamColors.Red));
            Competition.Participants.Add(new Driver("Claas", 0, new Car(0, 0, 10, false), IParticipant.TeamColors.Green));
            Competition.Participants.Add(new Driver("Dirk", 0, new Car(0, 0, 10, false), IParticipant.TeamColors.Yellow));
        }

        /// <summary>
        /// Adds tracks to the competition
        /// rules:
        ///  StartGrid is horizontal, direction going left,
        ///  Next round starts after the participant passes the first start grid
        /// </summary>
        public static void AddTracks() 
        {
            Competition.Tracks.Enqueue(new Track("track01 - bocht rechts", new SectionTypes[] { 
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Finish,
                SectionTypes.RightCorner
            }));
            Competition.Tracks.Enqueue(new Track("track02 - bocht links", new SectionTypes[] {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.Finish,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Finish,
                SectionTypes.LeftCorner
            }));
            Competition.Tracks.Enqueue(new Track("It's a key, I swear.", new SectionTypes[] {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Finish
            }));
        }

        /// <summary>
        /// Sets the next race for the competition, if available
        /// </summary>
        public static void NextRace() 
        {
            var track = Competition.NextTrack();
            if (track != null)
            {
                CurrentRace = new Race(track, Competition.Participants);
            }
        }
    }
}