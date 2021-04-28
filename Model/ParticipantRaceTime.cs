using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class ParticipantRaceTime : IParticipantResult<ParticipantRaceTime>
    {
        public string Name { get; set; }
        public TimeSpan Time { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="time"></param>
        public ParticipantRaceTime(string name, TimeSpan time)
        {
            Name = name;
            Time = time;
        }

        /// <summary>
        /// Adds to list
        /// </summary>
        /// <param name="tList"></param>
        public void Add(List<ParticipantRaceTime> tList)
        {
            tList.Add(new ParticipantRaceTime(Name, Time));
        }

        /// <summary>
        /// Returns name of highest score
        /// </summary>
        /// <param name="tList"></param>
        /// <returns></returns>
        public string BestParticipant(List<ParticipantRaceTime> tList)
        {
            return tList.OrderByDescending(p => p.Time).Last().Name;
        }


        /// <summary>
        /// Returns entire list
        /// </summary>
        /// <param name="tList"></param>
        /// <returns></returns>
        public List<ParticipantRaceTime> GetList(List<ParticipantRaceTime> tList)
        {
            return tList;
        }

    }
}
