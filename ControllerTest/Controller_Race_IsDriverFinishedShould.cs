using System.Linq;
using Controller;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    public class Controller_Race_IsDriverFinishedShould
    {
        private Competition _competition { get; set; }
        private Race _currentRace { get; set; }

        [SetUp]
        public void SetUp()
        {
            _competition = new Competition();
            AddParticipants();
            AddTracks();
            _currentRace = new Race(_competition.Tracks.Dequeue(), _competition.Participants);

            _currentRace.LapCount.Add(_competition.Participants.ElementAt(0), 1);
            _currentRace.LapCount.Add(_competition.Participants.ElementAt(1), 2);
        }

        [Test]
        public void IsDriverFinished_LapCountOne_ReturnFalse()
        {
            var result = _currentRace.IsDriverFinished(_competition.Participants.ElementAt(0));

            Assert.IsFalse(result);
        }

        [Test]
        public void IsDriverFinished_LapCountTwo_ReturnTrue()
        {
            var result = _currentRace.IsDriverFinished(_competition.Participants.ElementAt(1));

            Assert.IsTrue(result);
        }

        [Test]
        public void IsDriverFinished_NoValue_ReturnFalse()
        {
            var result = _currentRace.IsDriverFinished(_competition.Participants.ElementAt(2));

            Assert.IsFalse(result);
        }

        /// <summary>
        /// Adds participants to the competition
        /// </summary>
        public void AddParticipants()
        {
            _competition.Participants.Add(new Driver("Aakje", 0, new Car(0, 0, 10, false), IParticipant.TeamColors.Blue));
            _competition.Participants.Add(new Driver("Bert", 0, new Car(0, 0, 10, false), IParticipant.TeamColors.Red));
            _competition.Participants.Add(new Driver("Claas", 0, new Car(0, 0, 10, false), IParticipant.TeamColors.Green));
            _competition.Participants.Add(new Driver("Dirk", 0, new Car(0, 0, 10, false), IParticipant.TeamColors.Yellow));
        }

        /// <summary>
        /// Adds tracks to the competition
        /// rules:
        ///  StartGrid is horizontal, direction going left,
        ///  Next round starts after the participant passes the first start grid
        /// </summary>
        public void AddTracks()
        {
            _competition.Tracks.Enqueue(new Track("track01 - bocht rechts", new SectionTypes[] {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Finish,
                SectionTypes.RightCorner
            }));
            _competition.Tracks.Enqueue(new Track("track02 - bocht links", new SectionTypes[] {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.Finish,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Finish,
                SectionTypes.LeftCorner
            }));
            _competition.Tracks.Enqueue(new Track("It's a key, I swear.", new SectionTypes[] {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Finish
            }));
        }
    }
}
