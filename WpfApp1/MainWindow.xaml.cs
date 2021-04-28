using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Controller;
using Model;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Window _statisticsCompetitionWindow { get; set; }
        private Window _statisticsCurrentRaceWindow { get; set; }

        private int _startX { get; set; }
        private int _startY { get; set; }
        private int _width { get; set; }
        private int _height { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            Data.Initialize();

            InitializeComponent();
            
            Data.CurrentRace.Start();
            CalculateSizeOfMapAndCursorPosition();

            Race.RaceFinished += OnRaceFinished;
            Race.RoundFinished += OnRoundFinished;
            Race.DriversChanged += OnDriversChanged;
        }

        /// <summary>
        /// Adds result to datacontainer for round finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="rfea"></param>
        public void OnRoundFinished(object sender, RoundFinishedEventArgs rfea)
        {
            Data.Competition.ParticipantRoundTimeDataContainer.AddToList(new ParticipantRoundTime(rfea.LapCount, rfea.Name, rfea.Time));
        }

        /// <summary>
        /// OnRaceFinished event,
        ///  clears image cache,
        ///  fills datacontainers with information,
        ///  starts the next race
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnRaceFinished(object sender, RaceFinishedEventArgs args)
        {
            this.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    ImageController.ClearCache();
                    AddParticipantScoreAndTime(args);
                    Data.NextRace();
                    Data.CurrentRace.Start();
                    CalculateSizeOfMapAndCursorPosition();
                    Data.Competition.ParticipantRoundTimeDataContainer.GetList().Clear();
                }));

            Thread.Sleep(5000);
        }

        /// <summary>
        /// Fill datacontainers with information from race(s)
        /// </summary>
        /// <param name="args"></param>
        public static void AddParticipantScoreAndTime(RaceFinishedEventArgs args)
        {
            var position = 1;
            while (args.FinishedParticipants.Count > 0)
            {
                var score = 100 / position;
                var name = args.FinishedParticipants.Dequeue().Name;
                var timeSpan = args.FinishedTimeSpans.Dequeue();

                Data.Competition.ParticipantScoreDataContainer.AddToList(new ParticipantRaceScore(name, score));
                Data.Competition.ParticipantRaceTimeDataContainer.AddToList(new ParticipantRaceTime(name, timeSpan));

                position++;
            }
        }

        /// <summary>
        /// OnDriversChanged event, draws the track again
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDriversChanged(object sender, DriversChangedEventArgs args)
        {
            this.ImageName.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    this.ImageName.Source = null;
                    this.ImageName.Source = VisualizationWpf.DrawTrack(Data.CurrentRace.Track, _startX, _startY, _width, _height);
                }));
        }

        /// <summary>
        /// Calculates width and height of the race circuit
        /// </summary>
        private void CalculateSizeOfMapAndCursorPosition()
        {
            var (listX, listY) = VisualizationWpf.CalculateSizeOfMap(Data.CurrentRace.Track);

            _startX = listX.Min() < 0 ? Math.Abs(listX.Min()) : 0;
            _startY = listY.Min() < 0 ? Math.Abs(listY.Min() - 500) : 0;

            _width = ((listX.Min() < 0 ? Math.Abs(listX.Min()) : listX.Min()) + listX.Max()) + 1000;
            _height = ((listY.Min() < 0 ? Math.Abs(listY.Min()) : listY.Min()) + listY.Max()) + 1000;
        }

        /// <summary>
        /// Opens competition statistics menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_StatisticsCompetitionWindow_OnClick(object sender, RoutedEventArgs e)
        {
            _statisticsCompetitionWindow = new StatisticsCompetition();
            _statisticsCompetitionWindow.Show();
        }

        /// <summary>
        /// Opens race statistics menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_StatisticsCurrentRaceWindow_OnClick(object sender, RoutedEventArgs e)
        {
            _statisticsCurrentRaceWindow = new StatisticsCurrentRace();
            _statisticsCurrentRaceWindow.Show();
        }

        /// <summary>
        /// Closes the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Exit_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.Shutdown();
        }
    }
}
