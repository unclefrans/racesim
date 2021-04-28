using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class ParticipantRoundTime : IParticipantResult<ParticipantRoundTime>
    {
        public int Round { get; set; }
        public string Name { get; set; }
        public TimeSpan Time { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="round"></param>
        /// <param name="name"></param>
        /// <param name="time"></param>
        public ParticipantRoundTime(int round, string name, TimeSpan time)
        {
            Round = round;
            Name = name;
            Time = time;
        }

        /// <summary>
        /// Adds to list for round time,
        ///  if the name already exists means a round was already done, so get the previous time and abstract that from the new time
        ///  does this work with more than two rounds?
        /// </summary>
        /// <param name="tList">List</param>
        public void Add(List<ParticipantRoundTime> tList)
        {
            if (tList.Any(p => p.Name == this.Name))
            {
                tList.Add(new ParticipantRoundTime(Round, Name, this.Time -= tList.Find(p => p.Name == this.Name).Time));
            }
            else
            {
                tList.Add(new ParticipantRoundTime(Round, Name, Time));
            }
        }

        /// <summary>
        /// Returns best participant with highest score
        /// </summary>
        /// <param name="tList"></param>
        /// <returns></returns>
        public string BestParticipant(List<ParticipantRoundTime> tList)
        {
            return tList.OrderByDescending(p => p.Time).Last().Name;
        }

        /// <summary>
        /// Return entire list
        /// </summary>
        /// <param name="tList"></param>
        /// <returns></returns>
        public List<ParticipantRoundTime> GetList(List<ParticipantRoundTime> tList)
        {
            return tList;
        }
    }
}
