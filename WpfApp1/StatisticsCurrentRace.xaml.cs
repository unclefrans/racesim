using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using Controller;
using Model;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for StatisticsCurrentRace.xaml
    /// </summary>
    public partial class StatisticsCurrentRace : Window
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StatisticsCurrentRace()
        {
            InitializeComponent();
            
            this.ParticipantRoundTimeDataGrid.DataContext = Data.Competition.ParticipantRoundTimeDataContainer.GetList();

            Race.RoundFinished += OnRoundFinished;
            Race.RaceFinished += OnRaceFinished;
        }

        /// <summary>
        /// Event on race finished, resets displayed datagrid with results
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="rfea"></param>
        private void OnRaceFinished(object sender, RaceFinishedEventArgs rfea)
        {
            this.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    this.ParticipantRoundTimeDataGrid.DataContext = null;
                }));
        }

        /// <summary>
        /// Event on round finished, displays a datagrid with results
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="rfea"></param>
        private void OnRoundFinished(object sender, RoundFinishedEventArgs rfea)
        {
            this.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    this.ParticipantRoundTimeDataGrid.DataContext = null;
                    this.ParticipantRoundTimeDataGrid.DataContext = Data.Competition.ParticipantRoundTimeDataContainer.GetList();
                }));
        }
        
        /// <summary>
        /// Unbind events on closing of window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatisticsCurrentRace_OnClosing(object sender, CancelEventArgs e)
        {
            Race.RoundFinished -= OnRoundFinished;
            Race.RaceFinished -= OnRaceFinished;
        }
    }
}
