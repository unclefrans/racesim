using System;
using System.Linq;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    public class Model_DataContainer_AddToListShould
    {
        private Competition _competition { get; set; }

        [SetUp]
        public void SetUp()
        {
            _competition = new Competition();
        }

        [Test]
        public void AddToList_OneRoundResult_IfInList()
        {
            var roundResult1 = new ParticipantRoundTime(1, "participantname", TimeSpan.MaxValue);
            _competition.ParticipantRoundTimeDataContainer.AddToList(roundResult1);
            Assert.That(_competition.ParticipantRoundTimeDataContainer.GetList().Any(p => p.Name == "participantname"));
        }
    }
}
