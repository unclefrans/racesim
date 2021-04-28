using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Timer = System.Timers.Timer;

namespace Controller
{
    public class Race
    {
        public delegate void EventListener(object sender, DriversChangedEventArgs args);
        public static event EventListener DriversChanged;

        public delegate void RoundFinishedEventListener(object sender, RoundFinishedEventArgs args);
        public static event RoundFinishedEventListener RoundFinished;

        public delegate void RaceFinishedEventListener(object sender, RaceFinishedEventArgs args);
        public static event RaceFinishedEventListener RaceFinished;

        private Random _random { get; set; } = new Random(DateTime.Now.Millisecond);

        private readonly Dictionary<Section, SectionData> _positions = new Dictionary<Section, SectionData>();
        public Dictionary<IParticipant, int> LapCount = new Dictionary<IParticipant, int>();

        private readonly Queue<IParticipant> _finishedParticipants = new Queue<IParticipant>();
        private readonly Queue<TimeSpan> _finishedTimes = new Queue<TimeSpan>();

        public DateTime StartTime { get; set; } = DateTime.Now;
        public Track Track { get; set; }
        public List<IParticipant> Participants { get; set; }
        private Timer Timer { get; set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="track">Current track of the competition</param>
        /// <param name="participants">Participants of the competition</param>
        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;

            RandomizeEquipment();

            SetDriverStartingPosition(track, Participants);

            Timer = new Timer(500); 
            Timer.Elapsed += OnTimedEvent; 
        }
        
        /// <summary>
        /// Starts the timer
        /// </summary>
        public void Start()
        {
            Timer.Start();
        }

        /// <summary>
        /// If a new lap has started or not,
        ///  Also if a race is finished, should probably get this out of this method
        /// </summary>
        /// <param name="newLap"></param>
        /// <param name="participant"></param>
        public void NewLap(bool newLap, IParticipant participant)
        {
            if (!newLap) return;
            try
            {
                LapCount.Add(participant, 1);
            }
            catch (ArgumentException)
            {
                LapCount[participant]++;
            }

            var timeSpan = DateTime.Now - StartTime;
            LapCount.TryGetValue(participant, out var lapCount);

            RoundFinished?.Invoke(this, new RoundFinishedEventArgs() { LapCount = lapCount, Name = participant.Name, Time = timeSpan });

            if (!IsDriverFinished(participant)) return;
            RemoveDriverFromTrack(participant);
            _finishedParticipants.Enqueue(participant);
            _finishedTimes.Enqueue(timeSpan);
        }

        /// <summary>
        /// Is the race finished? (no participants left on track)
        /// </summary>
        /// <returns></returns>
        public bool IsRaceFinished()
        {
            foreach (var section in Track.Sections)
            {
                _positions.TryGetValue(section, out var sd);
                if (sd?.Right != null || sd?.Left != null)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Remove the driver from the current track if last round is done
        /// </summary>
        /// <param name="participant"></param>
        public void RemoveDriverFromTrack(IParticipant participant)
        {
            LapCount.Remove(participant);
            foreach (var section in Track.Sections)
            {
                _positions.TryGetValue(section, out var sd);
                if (sd?.Right != null && sd.Right == participant)
                {
                    sd.Right = null;
                }

                if (sd?.Left != null && sd.Left == participant)
                {
                    sd.Left = null;
                }
            }
        }

        /// <summary>
        /// If the driver is finished or not (amount of rounds, in this case two)
        /// </summary>
        /// <param name="participant"></param>
        /// <returns></returns>
        public bool IsDriverFinished(IParticipant participant)
        {
            if (!LapCount.TryGetValue(participant, out var value)) return false;
            return value >= 2;
        }

        /// <summary>
        /// Changes the drivers position,
        ///  If there's one already on the left position, try to move it to the right position,
        /// (could maybe use some work and remove the wetflooring?)
        /// </summary>
        public void ChangeDriverPosition()
        {
            const int sectionLength = 100;
            for (var i = Track.Sections.Count; i > 0; i--)
            {
                var sd = GetSectionData(Track.Sections.ElementAt(i - 1));


                // for new lap counter
                bool newLap;

                // forward section data, where the driver should get on
                SectionData fsd;
                if (i == Track.Sections.Count)
                {
                    fsd = GetSectionData(Track.Sections.ElementAt(0)); // start the lap again!
                    newLap = true;
                }
                else
                {
                    fsd = GetSectionData(Track.Sections.ElementAt(i));
                    newLap = false;
                }

                if (sd.Left != null && sd.Left.Equipment.IsBroken == false)
                {
                    sd.DistanceLeft += sd.Left.Equipment.Performance * sd.Left.Equipment.Speed;
                    if (sd.DistanceLeft > sectionLength)
                    {
                        // is there a driver already on the left section?
                        if (fsd.Left != null)
                        {
                            // is there also a driver on the right section?!
                            if (fsd.Right != null)
                            {
                                // stay in your current section but at max distance
                                sd.DistanceLeft = 100;
                            }
                            // else move to the right section
                            else
                            {
                                NewLap(newLap, sd.Left);
                                fsd.Right = sd.Left;
                                fsd.DistanceRight = sd.DistanceLeft % sectionLength;
                                sd.Left = null;
                                sd.DistanceLeft = 0;
                            }
                        }
                        // forward!
                        else
                        {
                            NewLap(newLap, sd.Left);
                            fsd.Left = sd.Left;
                            fsd.DistanceLeft = sd.DistanceLeft % sectionLength;
                            sd.Left = null;
                            sd.DistanceLeft = 0;
                        }
                    }
                }

                if (sd.Right != null && sd.Right.Equipment.IsBroken == false)
                {
                    sd.DistanceRight += sd.Right.Equipment.Performance * sd.Right.Equipment.Speed;
                    if (sd.DistanceRight > sectionLength)
                    {
                        // is there a driver already on the right section?
                        if (fsd.Right != null)
                        {
                            // is there also a driver on the left section?!
                            if (fsd.Left != null)
                            {
                                // stay in your current section but at max distance
                                sd.DistanceRight = 100;
                            }
                            // else move to the left section
                            else
                            {
                                NewLap(newLap, sd.Right);
                                fsd.Left = sd.Right;
                                fsd.DistanceLeft = sd.DistanceRight % sectionLength;
                                sd.Right = null;
                                sd.DistanceRight = 0;
                            }
                        }
                        // forward!
                        else
                        {
                            NewLap(newLap, sd.Right);
                            fsd.Right = sd.Right;
                            fsd.DistanceRight = sd.DistanceRight % sectionLength;
                            sd.Right = null;
                            sd.DistanceRight = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// OnTimeEvent,
        ///  changes the driver position and decides if the equipment is broken after moving,
        ///  unbinds all events, stops timer and calls for the race finished event when the race is finished
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        public void OnTimedEvent(object source, EventArgs args)
        {
            ChangeDriverPosition();
            DriversChanged?.Invoke(this, new DriversChangedEventArgs() { Track = Track });

            BreakDriverEquipment();

            if (!IsRaceFinished()) return;

            UnbindEvents();
            Timer.Stop();
            RaceFinished?.Invoke(this, new RaceFinishedEventArgs() 
            { 
                FinishedParticipants = _finishedParticipants, 
                FinishedTimeSpans = _finishedTimes
            });
        }

        /// <summary>
        /// Breaks the driver's equipment, some random chance (might need some work)
        /// </summary>
        public void BreakDriverEquipment()
        {
            foreach (var participant in Participants)
            {
                var totalQualityPerformance = (participant.Equipment.Quality * participant.Equipment.Performance);
                if (_random.Next(totalQualityPerformance) <= (totalQualityPerformance / 10))
                {
                    participant.Equipment.IsBroken = true;
                }
                else
                {
                    participant.Equipment.IsBroken = false;
                }
            }
        }
        
        /// <summary>
        /// unbinds the events
        /// </summary>
        public void UnbindEvents()
        {
            Timer.Elapsed -= OnTimedEvent;
        }

        /// <summary>
        /// Randomizes equipment
        /// </summary>
        public void RandomizeEquipment()
        {
            foreach (var participant in Participants)
            {
                participant.Equipment.Quality = _random.Next(3, 6);
                participant.Equipment.Performance = _random.Next(3, 6);
            }
        }

        /// <summary>
        /// Sets the driver's starting position on start grids (must have enough start grids available)
        /// </summary>
        /// <param name="track">Current track</param>
        /// <param name="participants">Participants</param>
        public void SetDriverStartingPosition(Track track, List<IParticipant> participants)
        {
            var startGrid = track.Sections.Where(section => section.SectionType == SectionTypes.StartGrid).ToList();

            var startStartGridIndex = 0;
            var left = true;
            foreach (Driver driver in participants)
            {
                if (left)
                {
                    var sd = GetSectionData(startGrid[startStartGridIndex]);
                    sd.Left = driver;
                    sd.DistanceLeft = 0;
                    left = false;
                }
                else
                {
                    var sd = GetSectionData(startGrid[startStartGridIndex]);
                    sd.Right = driver;
                    sd.DistanceRight = 0;
                    left = true;
                    startStartGridIndex++;
                }
            }
        }

        /// <summary>
        /// Gets the section data information
        /// </summary>
        /// <param name="section">Section</param>
        /// <returns>SectionData</returns>
        public SectionData GetSectionData(Section section)
        {
            if (_positions.TryGetValue(section, out var value))
            {
                return value;
            }
            else
            {
                var sd = new SectionData();
                _positions.Add(section, sd);
                return sd;
            }
        }
    }
}