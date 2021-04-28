using System;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    public class Model_DataContainer_DisplayBestParticipantShould
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
        public void DisplayBestParticipant_ParticipantScoreOneRaceResult_DisplayBestParticipant()
        {
            foreach (var participant in _competition.Participants)
            {
                _competition.ParticipantScoreDataContainer.AddToList(new ParticipantRaceScore(participant.Name, participant.Points));
            }

            var bestParticipant = _competition.ParticipantScoreDataContainer.DisplayBestParticipant();

            Assert.AreEqual("Aakje", bestParticipant);
        }

        [Test]
        public void DisplayBestParticipant_ParticipantRoundTimeOneRaceResult_DisplayBestParticipant()
        {
            foreach (var participant in _competition.Participants)
            {
                _competition.ParticipantRoundTimeDataContainer.AddToList(new ParticipantRoundTime(1, "Aakje", TimeSpan.MaxValue));
                _competition.ParticipantRoundTimeDataContainer.AddToList(new ParticipantRoundTime(1, "Bert", TimeSpan.MinValue));

            }

            var bestParticipant = _competition.ParticipantRoundTimeDataContainer.DisplayBestParticipant();

            Assert.AreEqual("Bert", bestParticipant);
        }

        [Test]
        public void DisplayBestParticipant_ParticipantRaceTimeTimeOneRaceResult_DisplayBestParticipant()
        {
            foreach (var participant in _competition.Participants)
            {
                _competition.ParticipantRaceTimeDataContainer.AddToList(new ParticipantRaceTime("Aakje", TimeSpan.MaxValue));
                _competition.ParticipantRaceTimeDataContainer.AddToList(new ParticipantRaceTime("Bert", TimeSpan.MinValue));
            }

            var bestParticipant = _competition.ParticipantRaceTimeDataContainer.DisplayBestParticipant();

            Assert.AreEqual("Bert", bestParticipant);
        }
    }
}
