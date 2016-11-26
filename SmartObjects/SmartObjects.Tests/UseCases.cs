using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

namespace SmartObjects.Tests
{
    [TestFixture]
    class UseCases
    {
        private const int NUMBER_OF_OBJECTS_PER_ITERATION = 100;


        [Test]
        public void When_creating_and_releasing_100_objects_over_10000_iterations_it_should_be_quicker_to_use_a_recycle_collection()
        {
            // Arrange
            var numberOfIterations = 10000;
            var recycleBin = new RecycleBin<object>(NUMBER_OF_OBJECTS_PER_ITERATION);
            var recycleCollection = new RecycleCollection<object>(recycleBin, new List<object>(NUMBER_OF_OBJECTS_PER_ITERATION));
            var regularCollection = new List<object>(NUMBER_OF_OBJECTS_PER_ITERATION);

            // Act
            var recycleCollectionTime = Execute(numberOfIterations, regularCollection, () => new object());
            var regularCollectionTime = Execute(numberOfIterations, recycleCollection, recycleBin.GetInstance);

            // Assert
            Assert.That(regularCollectionTime, Is.GreaterThan(recycleCollectionTime));
            Console.WriteLine($@"
    Recycle: {recycleCollectionTime}
    Regular: {regularCollectionTime}
");
        }

        private static long Execute(int numberOfIterations, ICollection<object> collection, Func<object> objectMake)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (numberOfIterations-- > 0)
            {
                for (var i = 0; i < NUMBER_OF_OBJECTS_PER_ITERATION; i++)
                {
                    collection.Add(objectMake());
                }

                collection.Clear();
            }

            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
    }
}
