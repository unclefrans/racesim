using System.Collections.Generic;
using System.Linq;


namespace Model
{
    public class ParticipantRaceScore : IParticipantResult<ParticipantRaceScore>
    {
        public string Name { get; set; }
        public int Score { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="score"></param>
        public ParticipantRaceScore(string name, int score)
        {
            Name = name;
            Score = score;
        }

        /// <summary>
        /// If the driver already has a score, just add it to the name, else add new one in list
        /// </summary>
        /// <param name="tList">List</param>
        public void Add(List<ParticipantRaceScore> tList)
        {
            if (tList.Any(p => p.Name == this.Name))
            {
                tList.Find(p => p.Name == this.Name).Score += this.Score;
            }
            else
            {
                tList.Add(new ParticipantRaceScore(Name, Score));
            }
        }

        /// <summary>
        /// Displays the best participant,
        ///  two have the same highest score? tough... Still gets the first one, no sharing allowed.
        /// </summary>
        /// <param name="tList">List</param>
        /// <returns>String, best participant of highest score</returns>
        public string BestParticipant(List<ParticipantRaceScore> tList)
        {
            return tList.OrderByDescending(p => p.Score).First().Name;
        }
        
        /// <summary>
        /// Displays the best participant highest score,
        ///  two have the same highest score? tough... Still gets the first one, no sharing allowed.
        /// </summary>
        /// <param name="tList">List</param>
        /// <returns>String, best score</returns>
        public int BestScore(List<ParticipantRaceScore> tList)
        {
            return tList.OrderByDescending(p => p.Score).First().Score;
        }

        /// <summary>
        /// Returns entire list
        /// </summary>
        /// <param name="tList"></param>
        /// <returns></returns>
        public List<ParticipantRaceScore> GetList(List<ParticipantRaceScore> tList)
        {
            return tList;
        }
    }
}
