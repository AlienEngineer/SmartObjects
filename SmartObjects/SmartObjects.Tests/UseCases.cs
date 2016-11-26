using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;

namespace SmartObjects.Tests
{
    [TestFixture]
    class UseCases
    {
        private int MAX_NUMBER_OF_ITERATIONS = 1000000;
        private const int NUMBER_OF_OBJECTS_PER_ITERATION = 100;

        //[Test]
        public void ExportTimes()
        {
            var file = new StreamWriter(File.Create(@"C:\MyProjects\SmartObjects\Test.csv"));
            for (int numberOfIterations = 0; numberOfIterations < MAX_NUMBER_OF_ITERATIONS; numberOfIterations += 10000)
            {

                // Arrange
                var recycleBin = new RecycleBin<object>(NUMBER_OF_OBJECTS_PER_ITERATION);
                var recycleCollection = new RecycleCollection<object>(recycleBin, new List<object>(NUMBER_OF_OBJECTS_PER_ITERATION));
                var regularCollection = new List<object>(NUMBER_OF_OBJECTS_PER_ITERATION);

                // Act
                var recycleCollectionTime = Execute(numberOfIterations, regularCollection, () => new object());
                var regularCollectionTime = Execute(numberOfIterations, recycleCollection, recycleBin.GetInstance);

                // Assert
                file.WriteLine($"{numberOfIterations};{recycleCollectionTime};{regularCollectionTime}");
            }
            file.Flush();
            file.Close();
        }

        [Test]
        public void When_creating_and_releasing_100_objects_over_x_iterations_it_should_be_quicker_to_use_a_recycle_collection(
            [Values(100, 1000, 10000)]
            int numberOfIterations)
        {
            // Arrange
            var recycleBin = new RecycleBin<object>(NUMBER_OF_OBJECTS_PER_ITERATION);
            var recycleCollection = new RecycleCollection<object>(recycleBin, new List<object>(NUMBER_OF_OBJECTS_PER_ITERATION));
            var regularCollection = new List<object>(NUMBER_OF_OBJECTS_PER_ITERATION);

            // Act
            var recycleCollectionTime = Execute(numberOfIterations, regularCollection, () => new object());
            var regularCollectionTime = Execute(numberOfIterations, recycleCollection, recycleBin.GetInstance);

            // Assert
            Console.WriteLine($@"
    Iterations: {numberOfIterations}
    Recycle:    {recycleCollectionTime}
    Regular:    {regularCollectionTime}
");
            Assert.That(regularCollectionTime, Is.GreaterThan(recycleCollectionTime));
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
            return stopwatch.ElapsedTicks;
        }
    }
}
