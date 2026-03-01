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
        Assert.Throws<InvalidOperationException>(() => {
            SafeList<int> list = new SafeList<int>(new []{1});
            foreach (var value in list) {
                list.Add(value + 1);
            }
        });
    }
}