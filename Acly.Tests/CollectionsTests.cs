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
    }
}
