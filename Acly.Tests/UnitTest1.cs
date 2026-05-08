using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Acly.Tests
{
    public class Tests
    {
        public class A
        {
        }
        public class Ba : A
        {
        }

        [Test]
        public void AssignableTest()
        {
            var a = typeof(A);
            var ba = typeof(Ba);

            Console.WriteLine(a.IsAssignableFrom(ba));
            Console.WriteLine(ba.IsAssignableFrom(a));
        }

        [Test]
        public void TestUnitedCollection()
        {
            RelayCollectionItemsFilter filter = new()
            {
                Filter = (c, i) =>
                {
                    if (i is int number)
                    {
                        return number % 2 == 0;
                    }

                    return false;
                }
            };
            var list1 = new ObservableCollection<int> { 1, 2, 3 };
            var list2 = new ObservableCollection<int> { 4, 5, 6 };
            var united = new UnitedCollection<ObservableCollection<int>, int>(filter, list1, list2);

            Console.WriteLine($"Count: {united.Count}");

            foreach (var value in united)
            {
                Console.WriteLine(value);
            }
        }
    }
}
