using Controller;
using Model;
using System.ComponentModel;

namespace WpfApp1
{
    public class DataContextMainWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string TrackName { get => $"Track: {Data.CurrentRace.Track.Name}"; }
        public string BestParticipant { get => DisplayBestDriver(); }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public DataContextMainWindow()
        {
            if (Data.CurrentRace != null)
            {
                Race.DriversChanged += OnDriversChanged;
            }
        }

        /// <summary>
        /// EventHandler when drivers have changed positions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDriversChanged(object sender, DriversChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }

        /// <summary>
        /// Displays the best participant
        /// </summary>
        /// <returns></returns>
        public string DisplayBestDriver()
        {

            var participantName = Data.Competition.ParticipantScoreDataContainer.DisplayBestParticipant();
            var participantScore = Data.Competition.ParticipantScoreDataContainer.DisplayBestScore();
            if (participantName != "" && participantScore != "")
                return $"Best overall participant: {participantName} with {participantScore} points.";
            else
                return $"No best overall participant yet, a race has to be finished first!";
        }
    }
}
