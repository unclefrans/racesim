﻿using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    public class Model_DataContainer_DisplayBestScoreShould
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
        public void DisplayBestParticipant_ParticipantScoreOneRaceResult_DisplayBestScore()
        {
            foreach (var participant in _competition.Participants)
            {
                _competition.ParticipantScoreDataContainer.AddToList(new ParticipantRaceScore(participant.Name, participant.Points));
            }

            var bestScore = _competition.ParticipantScoreDataContainer.DisplayBestScore();

            Assert.AreEqual("100", bestScore);
        }
    }
}
