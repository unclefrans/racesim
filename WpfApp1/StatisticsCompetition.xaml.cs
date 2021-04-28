using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using Controller;
using Model;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for StatisticsCompetition.xaml
    /// </summary>
    public partial class StatisticsCompetition : Window
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StatisticsCompetition()
        {
            InitializeComponent();

            this.ParticipantRaceScoreDataGrid.DataContext = Data.Competition.ParticipantScoreDataContainer.GetList();

            Race.RaceFinished += OnRaceFinished;
        }

        /// <summary>
        /// Event on race finished, clears the datagrid and fills it with new information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="rfea"></param>
        private void OnRaceFinished(object sender, RaceFinishedEventArgs rfea)
        {
            this.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    this.ParticipantRaceScoreDataGrid.DataContext = null;
                    this.ParticipantRaceScoreDataGrid.DataContext = Data.Competition.ParticipantScoreDataContainer.GetList();
                }));
        }

        /// <summary>
        /// Unbind events on closing of window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatisticsCompetition_OnClosing(object sender, CancelEventArgs e)
        {
            Race.RaceFinished -= OnRaceFinished;
        }
    }
}
