using Framework;
using NUnit.Framework;

public class PooledListTest {
    [Test]
    public void RecycleClearsAndReusesList() {
        PooledList<int> temp;
        using (PooledList<int> first = PooledList<int>.Get()) {
            first.Add(1);
            first.Add(2);
            temp = first;
        }

        PooledList<int> second = PooledList<int>.Get();
        try {
            Assert.AreSame(temp, second);
            Assert.AreEqual(0, second.Count);
        }
        finally {
            second.Dispose();
        }
    }
}
