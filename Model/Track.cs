using System.Collections.Generic;

namespace Model
{
    public class Track
    {
        public string Name { get; set; }
        public LinkedList<Section> Sections { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of track</param>
        /// <param name="sections">List of all sections</param>
        public Track(string name, SectionTypes[] sections)
        {
            Name = name;
            Sections = new LinkedList<Section>(SectionTypesToList(sections));
        }

        /// <summary>
        /// Adding of sectiontypes in a linkedlist
        /// </summary>
        /// <param name="sectionTypes">sections list</param>
        /// <returns>List of sections</returns>
        public LinkedList<Section> SectionTypesToList(SectionTypes[] sectionTypes)
        {
            var _sections = new LinkedList<Section>();
            foreach (var sectionType in sectionTypes)
            {
                var section = new Section(sectionType);
                _sections.AddLast(section);
            }
            return _sections;
        }
    }
}