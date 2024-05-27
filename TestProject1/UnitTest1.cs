using Cars;
using LAB_13;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CollectionHandlerEventArgs()
        {
            // Arrange
            string expectedChangeType = "Added";
            object expectedAffectedObject = new object();

            // Act
            var args = new CollectionHandlerEventArgs(expectedChangeType, expectedAffectedObject);

            // Assert
            Assert.AreEqual(expectedChangeType, args.ChangeType);
            Assert.AreEqual(expectedAffectedObject, args.AffectedObject);
        }
        [TestMethod]
        public void Journal()
        {
            // Arrange
            string expectedCollectionName = "TestCollection";
            string expectedChangeType = "Added";
            string expectedAffectedObjectData = "Car";

            // Act
            var journalEntry = new JournalEntry(expectedCollectionName, expectedChangeType, expectedAffectedObjectData);

            // Assert
            Assert.AreEqual(expectedCollectionName, journalEntry.CollectionName);
            Assert.AreEqual(expectedChangeType, journalEntry.ChangeType);
            Assert.AreEqual(expectedAffectedObjectData, journalEntry.AffectedObjectData);
        }
        [TestMethod]
        public void AddEntry_JournalEntry()
        {
            // Arrange
            var journal = new Journal();
            var source = new MyObservableCollection<Car>(1); // Создаем объект источника
            var car = new Car(); // Создаем новый объект Car
            var args = new CollectionHandlerEventArgs("Added", car); // Создаем аргументы события

            // Act
            journal.AddEntry(source, args);

            // Assert
            Assert.AreEqual(1, journal.Entries.Count);
            Assert.AreEqual("MyObservableCollection`1", journal.Entries[0].CollectionName);
            Assert.AreEqual("Added", journal.Entries[0].ChangeType);
            Assert.AreEqual(typeof(Car), car.GetType());

        }
        [TestMethod]
        public void Add_ShouldInvokeCollectionCountChangedEvent()
        {
            // Arrange
            var collection = new MyObservableCollection<Car>(1);
            bool eventRaised = false;
            collection.CollectionCountChanged += (sender, args) => eventRaised = true;

            // Act
            collection.Add(new Car());


            // Assert
            Assert.IsTrue(eventRaised);
        }

        [TestMethod]
        public void Remove_ShouldInvokeCollectionCountChangedEvent()
        {
            // Arrange
            var collection = new MyObservableCollection<Car>(1);
            var car = new Car();
            collection.Add(car);
            bool eventRaised = false;
            collection.CollectionCountChanged += (sender, args) => eventRaised = true;

            // Act
            collection.Remove(car);

            // Assert
            Assert.IsTrue(eventRaised);
        }
        [TestMethod]
        public void IndexerSetter_ShouldInvokeCollectionReferenceChangedEvent()
        {
            // Arrange
            var collection = new MyObservableCollection<Car>(1);
            var car1 = new Car();
            var car2 = new Car();
            collection.Add(car1);
            bool eventRaised = false;
            collection.CollectionReferenceChanged += (sender, args) => eventRaised = true;

            // Act
            collection[car1] = car2;

            // Assert
            Assert.IsTrue(eventRaised);
        }
    }
}