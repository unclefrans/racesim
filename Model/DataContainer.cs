using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// Generic class datacontainer, this is where I had a lot of trouble working it out.
    /// I think I got it now.
    /// </summary>
    /// <typeparam name="T">Everything that is part of IParticipantResult</typeparam>
    public class DataContainer<T> where T : IParticipantResult<T>
    {
        private List<T> _list = new List<T>();

        /// <summary>
        /// Add to list depending on what template
        /// </summary>
        /// <param name="tList">List</param>
        public void AddToList(T tList)
        {
            tList.Add(_list);
        }

        /// <summary>
        /// Shows the best participant depending on what template
        /// </summary>
        /// <returns>String, either nothing or the best participant</returns>
        public string DisplayBestParticipant()
        {
            return _list.Count == 0 ? "" : _list[0].BestParticipant(_list);
        }

        /// <summary>
        /// Returns the list depending on what template
        /// </summary>
        /// <returns>List, a full list or empty</returns>
        public List<T> GetList()
        {
            return _list.Count > 0 ? _list[0].GetList(_list) : _list;
        }

        /// <summary>
        /// Shows the best score of best participant depending on what template 
        /// </summary>
        /// <returns>String, either nothing or best score</returns>
        public string DisplayBestScore()
        {
            return _list.Count == 0 ? "" : _list[0].BestScore(_list).ToString();
        }
    }
}
