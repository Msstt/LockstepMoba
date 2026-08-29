using System;
using Framework;
using NUnit.Framework;

public class SafeListTest {
    [Test]
    public void Test1() {
        SafeList<int> list = new SafeList<int>(new []{1, 2, 3, 4});
        int sum = 0;
        foreach (var value in list) {
            sum += value;
            list.Remove(value + 2);
        }
        Assert.AreEqual(sum, 3);
    }
    
    [Test]
    public void Test2() {
        SafeList<int> list = new SafeList<int>(new []{1, 2, 3, 4});
        int sum = 0;
        foreach (var value in list) {
            sum += value;
            list.Remove(value - 2);
        }
        Assert.AreEqual(sum, 10);
        foreach (var value in list) {
            sum += value;
            list.Remove(value - 2);
        }
        Assert.AreEqual(sum, 17);
    }
    
    [Test]
    public void Test3() {
        SafeList<int> list = new SafeList<int>(new []{1, 2});
        int sum = 0;
        foreach (var value in list) {
            sum += value;
            if (value <= 2) {
                list.Add(value + 2);
            }
        }
        Assert.AreEqual(sum, 10);
    }
    
    [Test]
    public void Test4() {
        SafeList<int> list = new SafeList<int>(new []{1});
        Assert.Throws<InvalidOperationException>(() => {
            foreach (var value in list) {
                list.Add(value + 1);
            }
        });
        Assert.DoesNotThrow(() => {
            var enumerator = list.GetEnumerator();
            enumerator.Dispose();
        });
    }

    [Test]
    public void FirstDoesNotBlockNextEnumeration() {
        SafeList<int> list = new SafeList<int>(new []{1, 2, 3});

        Assert.AreEqual(1, list.First());

        int sum = 0;
        foreach (var value in list) {
            sum += value;
        }
        Assert.AreEqual(6, sum);
    }

    [Test]
    public void BreakAppliesChangesAndDoesNotBlockNextEnumeration() {
        SafeList<int> list = new SafeList<int>(new []{1, 2, 3});

        foreach (var value in list) {
            list.Remove(2);
            list.Add(4);
            break;
        }

        int sum = 0;
        foreach (var value in list) {
            sum += value;
        }
        Assert.AreEqual(8, sum);
    }

    [Test]
    public void ExceptionDoesNotBlockNextEnumeration() {
        SafeList<int> list = new SafeList<int>(new []{1, 2, 3});

        Assert.Throws<InvalidOperationException>(() => {
            foreach (var _ in list) {
                throw new InvalidOperationException();
            }
        });

        int sum = 0;
        foreach (var value in list) {
            sum += value;
        }
        Assert.AreEqual(6, sum);
    }
}
