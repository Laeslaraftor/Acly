using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Acly.Tests
{
    public class CollectionsTests
    {
        [Test]
        public void TestUnitedCollection()
        {
            ObservableObject.SharedDispatcher = new Dispatcher();

            ObservableCollection<string> c1 = ["item 1", "item 2", "item 3"];
            ObservableCollection<string> c2 = [];
            UnitedCollection<ObservableCollection<string>, string> united = new(c1, c2);

            c2.Add("1 item");
            c2.Add("2 item");
            c2.Add("3 item");

            foreach (var item in united)
            {
                Console.WriteLine(item);
            }
        }
        [Test]
        public void ReflectionListTest()
        {
            EditableCollection<string> items = ["item 1", "item 2", "item 3"];
            ReflectionList list = new(items);

            list.Insert(1, "inserted item");

            foreach (var item in list )
            {
                Console.WriteLine(item);
            }
        }
        [Test]
        public void TestCollectionSynchronizer()
        {
            EditableCollection<string> collection1 = ["1", "22", "333"];
            EditableCollection<int> collection2 = [];
            CollectionSynchronizer<string, int> sync = new(collection1, collection2, new TestConverter());

            collection1.Remove("1");

            void PrintCollection<T>(IEnumerable<T> values)
            {
                foreach (var value in values)
                {
                    Console.WriteLine(value);
                }
            }

            Console.WriteLine("collection 1:");
            PrintCollection(collection1);
            Console.WriteLine("collection 2:");
            PrintCollection(collection2);
        }

        private class Dispatcher : IDispatcher
        {
            public void Dispatch(Action action)
            {
                action();
            }
            public async Task DispatchAsync(Action action)
            {
                await Task.Delay(20);
                action();
            }
        }
        private class TestConverter : IValueConverter<string, int>
        {
            public int Convert(string value)
            {
                return value.Length;
            }
            public string ConvertBack(int value)
            {
                return value.ToString();
            }
        }
    }
}
