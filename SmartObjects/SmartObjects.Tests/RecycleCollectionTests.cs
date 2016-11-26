using System.Collections;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SmartObjects.Tests.Helpers;

namespace SmartObjects.Tests
{
    [TestFixture]
    class RecycleCollectionTests : UnderTest<RecycleCollection<object>>
    {
        protected override void SetDependencies(IUnderTest<RecycleCollection<object>> subject)
        {
            subject.DependsOn<IRecycleBin<object>>();
            subject.DependsOn<ICollection<object>>();
        }

        protected override RecycleCollection<object> MakeSubject()
        {
            return new RecycleCollection<object>(
                Dependency<IRecycleBin<object>>().Object,
                Dependency<ICollection<object>>().Object
            );
        }

        [Test]
        public void When_clearing_list_it_should_store_dirty_instances_of_cleared_objects_in_recycle_bin()
        {
            // Arrange
            var obj1 = new object();
            var obj2 = new object();
            Subject.Add(obj1);
            Subject.Add(obj2);

            // Act
            Subject.Clear();

            // Assert
            Verify<IRecycleBin<object>>(e => e.Recycle(Dependency<ICollection<object>>().Object), Times.Once);
        }

        [Test]
        public void When_adding_items_it_should_add_items_to_underlying_collection()
        {
            // Arrange
            var obj1 = new object();

            // Act
            Subject.Add(obj1);

            // Assert
            Verify<ICollection<object>>(e => e.Add(obj1), Times.Once);
        }

        [Test]
        public void When_clearing_list_it_should_clear_list_to_underlying_collection()
        {
            // Act
            Subject.Clear();

            // Assert
            Verify<ICollection<object>>(e => e.Clear(), Times.Once);
        }

        [Test]
        public void When_removing_list_it_should_remove_list_to_underlying_collection()
        {
            // Arrange
            var obj1 = new object();

            // Act
            var result = Subject.Remove(obj1);

            // Assert
            Verify<ICollection<object>>(e => e.Remove(obj1), Times.Once);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void When_removing_list_it_should_store_removed_item_in_recycle_bin()
        {
            // Arrange
            var obj1 = new object();

            // Act
            var result = Subject.Remove(obj1);

            // Assert
            Verify<IRecycleBin<object>>(e => e.Recycle(obj1), Times.Once);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void When_verifying_list_contains_item_it_should_verify_on_underlying_collection()
        {
            // Arrange
            var obj1 = new object();

            // Act
            var result = Subject.Contains(obj1);

            // Assert
            Verify<ICollection<object>>(e => e.Contains(obj1), Times.Once);
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void When_copying_items_to_array_it_should_copy_items_from_underlying_collection()
        {
            // Arrange
            object[] array = { new object() };

            // Act
            Subject.CopyTo(array, 1531543848);

            // Assert
            Verify<ICollection<object>>(e => e.CopyTo(array, 1531543848), Times.Once);
        }

        [Test]
        public void When_getting_enumerator_it_should_return_enumerator_from_underlying_collection()
        {
            // Arrange
            var enumerator = new Mock<IEnumerator<object>>().Object;

            Setup<ICollection<object>, IEnumerator<object>>(e => e.GetEnumerator())
                .Returns(enumerator);

            // Act
            var result = Subject.GetEnumerator();

            // Assert
            Assert.That(result, Is.EqualTo(enumerator));
        }

        [Test]
        public void When_getting_non_generic_enumerator_it_should_return_enumerator_from_underlying_collection()
        {
            // Arrange
            var enumerator = new Mock<IEnumerator<object>>().Object;

            Setup<ICollection<object>, IEnumerator<object>>(e => e.GetEnumerator())
                .Returns(enumerator);

            // Act
            var result = ((IEnumerable)Subject).GetEnumerator();

            // Assert
            Assert.That(result, Is.EqualTo(enumerator));
        }


        [Test]
        public void When_getting_list_count_it_should_get_count_from_underlying_collection()
        {
            // Arrange
            Setup<ICollection<object>, int>(e => e.Count).Returns(153254);

            // Act
            var count = Subject.Count;

            // Assert
            Assert.That(count, Is.EqualTo(153254));
        }

        [Test]
        public void When_getting_list_IsReadOnly_it_should_get_IsReadOnly_from_underlying_collection()
        {
            // Arrange
            Setup<ICollection<object>, bool>(e => e.IsReadOnly).Returns(true);

            // Act
            var isReadOnly = Subject.IsReadOnly;

            // Assert
            Assert.That(isReadOnly, Is.EqualTo(true));
        }

    }
}