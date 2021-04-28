using Model;
using NUnit.Framework;

namespace ControllerTest
{
    class Model_Competition_SectionTypesToListShould
    {
        private Competition _competition;

        [SetUp]
        public void SetUp()
        {
            _competition = new Competition();
        }

        [Test]
        public void SectionTypesToList_List_ReturnNotNull()
        {
            var track = new Track("track1", new SectionTypes[] { });
            Assert.IsNotNull(track);
        }
    }
}