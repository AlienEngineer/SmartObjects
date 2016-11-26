using System.Linq;
using NUnit.Framework;
using SmartObjects.Tests.Helpers;

namespace SmartObjects.Tests
{
    [TestFixture]
    class RecycleBinTests : UnderTest<RecycleBin<object>>
    {
        private const int MAX_CAPACITY = 2;
        protected override void SetDependencies(IUnderTest<RecycleBin<object>> subject) { }

        protected override RecycleBin<object> MakeSubject()
        {
            return new RecycleBin<object>(MAX_CAPACITY);
        }

        [Test]
        public void When_recycling_an_item_it_should_keep_that_object()
        {
            // Arrange
            var item = new object();

            // Act
            Subject.Recycle(item);

            // Assert
            Assert.That(Subject.Contains(item), Is.True);
        }

        [Test]
        public void When_recycling_a_sequence_of_item_it_should_keep_all_objects()
        {
            // Arrange
            var list = new[]
            {
                new object(), new object()
            };

            // Act
            Subject.Recycle(list);

            // Assert
            Assert.That(Subject.Contains(list[0]), Is.True);
            Assert.That(Subject.Contains(list[1]), Is.True);
        }

        [Test]
        public void When_recycling_an_item_and_queued_list_is_greater_than_threashold_it_should_not_keep_those_objects()
        {
            // Arrange
            var item = new object();
            Subject.Recycle(Enumerable.Repeat(new object(), MAX_CAPACITY));

            // Act
            Subject.Recycle(item);

            // Assert
            Assert.That(Subject.Contains(item), Is.False);
        }

        [Test]
        public void When_recycling_items_and_queued_list_is_greater_than_threashold_it_should_not_keep_those_objects()
        {
            // Arrange
            var list = new[]
            {
                new object(), new object()
            };
            Subject.Recycle(Enumerable.Repeat(new object(), MAX_CAPACITY));

            // Act
            Subject.Recycle(list);

            // Assert
            Assert.That(Subject.Contains(list[0]), Is.False);
            Assert.That(Subject.Contains(list[1]), Is.False);
        }

        [Test]
        public void When_purging_recycle_bin_it_should_release_all_objects()
        {
            // Arrange
            Subject.Recycle(Enumerable.Repeat(new object(), MAX_CAPACITY));

            // Act
            Subject.Purge();

            // Assert
            Assert.That(Subject.IsEmpty, Is.True);
        }
    }
}