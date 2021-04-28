using System.Collections.Generic;

namespace Model
{
    public interface IParticipantResult<T>
    {
        public void Add(List<T> tList)
        {

        }

        public string BestParticipant(List<T> tList)
        {
            return "";
        }

        public List<T> GetList(List<T> tList)
        {
            return tList;
        }

        public int BestScore(List<T> tList)
        {
            return 0;
        }
    }
}
