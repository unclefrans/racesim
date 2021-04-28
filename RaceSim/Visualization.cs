using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RaceSim
{
    /// <summary>
    /// Visualization of the console sim
    /// </summary>
    public static class Visualization
    {
        #region graphics
        private static string[] _startGridHorizontal = {
            "----",
            "  1>",
            " 2> ",
            "----" };
        private static string[] _startGridVertical = {
            "|^ |",
            "|1^|",
            "| 2|",
            "|  |" };
        private static string[] _leftCornerHorizontal = {
            "   |",
            " 1 |",
            "  2|",
            "--- " };
        private static string[] _leftCornerVertical = {
            "--- ",
            " 1 |",
            "  2|",
            "   |" };
        private static string[] _rightCornerHorizontal = {
            "--- ",
            " 2 |",
            "  1|",
            "   |" };
        private static string[] _rightCornerVertical = {
            " ---",
            "| 1 ",
            "|2  ",
            "|   " };
        private static string[] _straightHorizontal = {
            "----",
            "  1 ",
            " 2  ",
            "----" };
        private static string[] _straightVertical = {
            "|  |",
            "|1 |",
            "| 2|",
            "|  |" };
        private static string[] _finishHorizontal = {
            "----",
            "  1#",
            " 2# ",
            "----" };
        private static string[] _finishVertical = {
            "|# |",
            "|1#|",
            "| 2|",
            "|  |" };
        #endregion

        private static int _cursPosX;
        private static int _cursPosY;
        private static int _calcX;
        private static int _calcY;
        private static int _direction;
        private static bool _reverseArray;
        private static bool _reverseString;
        private static readonly int LengthSectionType = _startGridHorizontal.Length;

        private static List<int> _xList;
        private static List<int> _yList;
        private static List<bool> _reverseArrayList;
        private static List<bool> _reverseStringList;
        private static List<string[]> _sectionList;

        private static int _differenceX;
        private static int _differenceY;

        /// <summary>
        /// Initializer with event handlers
        /// </summary>
        public static void Initialize()
        {
            _cursPosX = 0;
            _cursPosY = 0;
            _calcX = 0;
            _calcY = 0;
            _direction = 1;
            _reverseArray = false;
            _reverseString = false;

            _xList = new List<int>();
            _yList = new List<int>();
            _reverseArrayList = new List<bool>();
            _reverseStringList = new List<bool>();
            _sectionList = new List<string[]>();

            Race.DriversChanged += OnDriversChanged;
            Race.RoundFinished += OnRoundFinished;
            Race.RaceFinished += OnRaceFinished;
        }

        /// <summary>
        /// If round is finished, add driver to data container
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="rfea">Driver information</param>
        private static void OnRoundFinished(object sender, RoundFinishedEventArgs rfea)
        {
            Data.Competition.ParticipantRoundTimeDataContainer.AddToList(new ParticipantRoundTime(rfea.LapCount, rfea.Name, rfea.Time));
        }

        /// <summary>
        /// If race is finished, add drivers information to datacontainer,
        ///  clear the console form the track and display the best driver,
        ///  after five seconds, clear console again and then run the next race
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args">Driver information</param>
        private static void OnRaceFinished(object sender, RaceFinishedEventArgs args)
        {
            AddParticipantScoreAndTime(args);

            Console.Clear();
            DisplayBestDriver();
            Thread.Sleep(5000);

            Console.Clear();
            Console.Clear();

            Data.NextRace();
            Console.WriteLine(Data.CurrentRace.Track.Name);
            Data.CurrentRace.Start();
            Initialize();
        }

        /// <summary>
        /// Displays the best driver with highest score
        /// </summary>
        private static void DisplayBestDriver()
        {
            Console.WriteLine($"Best participant (highest score): {Data.Competition.ParticipantScoreDataContainer.DisplayBestParticipant()}");
        }

        /// <summary>
        /// Draws the track after drivers changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void OnDriversChanged(object sender, DriversChangedEventArgs args)
        {
            DrawTrack(args.Track);
        }

        /// <summary>
        /// Adds driver information to datacontainers
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
        /// Firstly, calculates how big the map needs to be,
        /// Then draws the track, also the drivers if they're on that section
        /// </summary>
        /// <param name="track"></param>
        public static void DrawTrack(Track track)
        {
            CalculateStartingPosition(track);
            _differenceX = _xList.Min() < 0 ? Math.Abs(_xList.Min()) : 0;
            _differenceY = _yList.Min() < 0 ? Math.Abs(_yList.Min()) : 0;

            for (var i = 0; i < track.Sections.Count; i++)
            {
                var participant1 = Data.CurrentRace.GetSectionData(track.Sections.ElementAt(i)).Left;
                var participant2 = Data.CurrentRace.GetSectionData(track.Sections.ElementAt(i)).Right;

                _cursPosX = _xList[i] + _differenceX;
                _cursPosY = _yList[i] + _differenceY + 1; // +1 to display track name top;
                Console.SetCursorPosition(_cursPosX, _cursPosY);
                var section = DrawDrivers(_sectionList[i], participant1, participant2);
                DrawSection(section, _reverseArrayList[i], _reverseStringList[i], _cursPosX, _cursPosY);
            }
        }

        /// <summary>
        /// Draws the section
        /// </summary>
        /// <param name="sectionType">SectionType</param>
        /// <param name="reverseArray">Reverse the array or not</param>
        /// <param name="reverseString">Reverse the string or not</param>
        /// <param name="x">Draw at x</param>
        /// <param name="y">Draw at y</param>
        private static void DrawSection(string[] sectionType, bool reverseArray, bool reverseString, int x, int y)
        {
            if (reverseArray)
                Array.Reverse(sectionType);
            foreach (var line in sectionType)
            {
                Console.SetCursorPosition(x, y);
                Console.WriteLine(reverseString ? ReverseString(line) : line);
                y++;
            }
            if (reverseArray)
                Array.Reverse(sectionType);
        }

        /// <summary>
        /// Race looping > = +4, < = -4, ^ = -4, \/ = +4 and push in list
        ///  start grid is always horizontal and will move to the right direction (1)
        ///  so when it goes into minus (outside of the window, it should add how much outside of it is to properly fill the track on console)
        /// </summary>
        /// <param name="track"></param>
        private static void CalculateStartingPosition(Track track)
        {
            _calcX = 0;
            _calcY = 0;
            _direction = 1;

            for (var i = 0; i < track.Sections.Count; i++)
            {
                var SectionType = track.Sections.ElementAt(i).SectionType;
                switch (SectionType)
                {
                    case SectionTypes.StartGrid:
                        _calcX += LengthSectionType;
                        _sectionList.Add(_startGridHorizontal);
                        break;
                    case SectionTypes.RightCorner:
                        _sectionList.Add(CalculateRightCornerSection());
                        break;
                    case SectionTypes.LeftCorner:
                        _sectionList.Add(CalculateLeftCornerSection());
                        break;
                    case SectionTypes.Straight:
                        _sectionList.Add(CalculateStraightSection(SectionType));
                        break;
                    case SectionTypes.Finish:
                        _sectionList.Add(CalculateStraightSection(SectionType));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _xList.Add(_calcX);
                _yList.Add(_calcY);
                _reverseArrayList.Add(_reverseArray);
                _reverseStringList.Add(_reverseString);
            }
        }

        /// <summary>
        /// SectionType going which direction?
        /// </summary>
        /// <param name="sectionType"></param>
        /// <returns>SectionType depending on what direction</returns>
        private static string[] CalculateStraightSection(SectionTypes sectionType)
        {
            switch (_direction)
            {
                case 1:
                    _reverseArray = false;
                    _reverseString = false;
                    _calcX += LengthSectionType;
                    return sectionType == SectionTypes.Straight ? _straightHorizontal : _finishHorizontal;
                case 2:
                    _reverseArray = true;
                    _reverseString = true;
                    _calcY += LengthSectionType;
                    return sectionType == SectionTypes.Straight ? _straightVertical : _finishVertical;
                case 3:
                    _reverseArray = false;
                    _reverseString = true;
                    _calcX -= LengthSectionType;
                    return sectionType == SectionTypes.Straight ? _straightHorizontal : _finishHorizontal;
                default:
                    _reverseArray = false;
                    _reverseString = false;
                    _calcY -= LengthSectionType;
                    return sectionType == SectionTypes.Straight ? _straightVertical : _finishVertical;
            }
        }

        /// <summary>
        /// SectionType going which direction?
        /// </summary>
        /// <param name="sectionType"></param>
        /// <returns>SectionType depending on what direction</returns>
        private static string[] CalculateRightCornerSection()
        {
            switch (_direction)
            {
                case 1:
                    _reverseArray = false;
                    _reverseString = false;
                    _calcX += LengthSectionType;
                    _direction = 2;
                    return _rightCornerHorizontal;
                case 2:
                    _reverseArray = true;
                    _reverseString = true;
                    _calcY += LengthSectionType;
                    _direction = 3;
                    return _rightCornerVertical;
                case 3:
                    _reverseArray = true;
                    _reverseString = true;
                    _calcX -= LengthSectionType;
                    _direction = 0;
                    return _rightCornerHorizontal;
                default:
                    _reverseArray = false;
                    _reverseString = false;
                    _calcY -= LengthSectionType;
                    _direction = 1;
                    return _rightCornerVertical;
            }
        }

        /// <summary>
        /// SectionType going which direction?
        /// </summary>
        /// <param name="sectionType"></param>
        /// <returns>SectionType depending on what direction</returns>
        private static string[] CalculateLeftCornerSection()
        {
            switch (_direction)
            {
                case 1:
                    _reverseArray = false;
                    _reverseString = false;
                    _calcX += LengthSectionType;
                    _direction = 0; 
                    return _leftCornerHorizontal;
                case 2:
                    _reverseArray = true;
                    _reverseString = true;
                    _calcY += LengthSectionType;
                    _direction = 1;
                    return _leftCornerVertical;
                case 3:
                    _reverseArray = true;
                    _reverseString = true;
                    _calcX -= LengthSectionType;
                    _direction = 2; 
                    return _leftCornerHorizontal;
                default:
                    _reverseArray = false;
                    _reverseString = false;
                    _calcY -= LengthSectionType;
                    _direction = 3; 
                    return _leftCornerVertical;
            }
        }

        /// <summary>
        /// Reverses a string
        /// </summary>
        /// <param name="s">String to reverse</param>
        /// <returns>Reversed string</returns>
        private static string ReverseString(string s)
        {
            var arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        /// <summary>
        /// Draws the drivers,
        ///  replaces the 1 or 2 on the graphics with either the name or nothing of the driver
        /// </summary>
        /// <param name="sectionType">Which sectiontype</param>
        /// <param name="participant1">Driver</param>
        /// <param name="participant2">Driver</param>
        /// <returns></returns>
        private static string[] DrawDrivers(string[] sectionType, IParticipant participant1, IParticipant participant2)
        {
            string replaceOne, replaceTwo;

            if (participant1 != null)
            {
                replaceOne = participant1.Equipment.IsBroken ? participant1.Name.ToCharArray()[0].ToString().ToLower() : participant1.Name.ToCharArray()[0].ToString();
            }
            else
            {
                replaceOne = " ";
            }

            if (participant2 != null)
            {
                replaceTwo = participant2.Equipment.IsBroken ? participant2.Name.ToCharArray()[0].ToString().ToLower() : participant2.Name.ToCharArray()[0].ToString();
            }
            else
            {
                replaceTwo = " ";
            }

            sectionType = sectionType.Select(x => x.Replace("1", replaceOne)).ToArray();
            sectionType = sectionType.Select(x => x.Replace("2", replaceTwo)).ToArray();
            return sectionType;
        }
    }
}