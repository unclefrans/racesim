using NUnit.Framework;
using Model;

namespace ControllerTest
{
    [TestFixture]
    public class Model_Competition_NextTrackShould
    {
        private Competition _competition { get; set; }

        [SetUp]
        public void SetUp()
        {
            _competition = new Competition();
        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            var result =_competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            var track = new Track("track1", new SectionTypes[] { });
            _competition.Tracks.Enqueue(track);
            var result = _competition.NextTrack();
            Assert.AreEqual(result, track);
        }

        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            var track = new Track("track1", new SectionTypes[] { });
            _competition.Tracks.Enqueue(track);
            var result = _competition.NextTrack();
            result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            var track1 = new Track("track1", new SectionTypes[] { });
            var track2 = new Track("track2", new SectionTypes[] { });

            _competition.Tracks.Enqueue(track1);
            _competition.Tracks.Enqueue(track2);

            var result = _competition.NextTrack();

            Assert.IsNotNull(result);
            result = _competition.NextTrack();

            Assert.IsNotNull(result);
            result = _competition.NextTrack();

            Assert.IsNull(result);
        }
    }

}
