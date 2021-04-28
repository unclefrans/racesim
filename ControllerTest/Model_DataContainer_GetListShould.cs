using NUnit.Framework;
using System;
using Model;

namespace ControllerTest
{
    [TestFixture]
    public class Model_DataContainer_GetListShould
    {
        private Competition _competition { get; set; }

        [SetUp]
        public void SetUp()
        {
            _competition = new Competition();

            _competition.Participants.Add(new Driver("Aakje", 100, new Car(0, 0, 10, false), IParticipant.TeamColors.Blue));
            _competition.Participants.Add(new Driver("Bert", 75, new Car(0, 0, 10, false), IParticipant.TeamColors.Red));
            _competition.Participants.Add(new Driver("Claas", 50, new Car(0, 0, 10, false), IParticipant.TeamColors.Green));
            _competition.Participants.Add(new Driver("Dirk", 25, new Car(0, 0, 10, false), IParticipant.TeamColors.Yellow));
        }

        [Test]
        public void GetList_ParticipantTimeOneRaceResult_ListNotEmpty()
        {
            foreach (var participant in _competition.Participants)
            {
                _competition.ParticipantRaceTimeDataContainer.AddToList(new ParticipantRaceTime(participant.Name, TimeSpan.MaxValue));
            }

            var list = _competition.ParticipantRaceTimeDataContainer.GetList();

            CollectionAssert.IsNotEmpty(list);
        }

        [Test]
        public void GetList_ParticipantTimeOneRaceResult_ListIsEmpty()
        {
            var list = _competition.ParticipantRaceTimeDataContainer.GetList();

            CollectionAssert.IsEmpty(list);
        }

        [Test]
        public void GetList_ParticipantScoreOneRaceResult_ListNotEmpty()
        {
            foreach (var participant in _competition.Participants)
            {
                _competition.ParticipantScoreDataContainer.AddToList(new ParticipantRaceScore(participant.Name, participant.Points));
            }

            var list = _competition.ParticipantScoreDataContainer.GetList();

            CollectionAssert.IsNotEmpty(list);
        }

        [Test]
        public void GetList_ParticipantScoreOneRaceResult_ListIsEmpty()
        {
            var list = _competition.ParticipantScoreDataContainer.GetList();

            CollectionAssert.IsEmpty(list);
        }
    }
}
