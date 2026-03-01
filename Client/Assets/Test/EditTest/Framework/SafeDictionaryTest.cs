using System;
using Framework;
using NUnit.Framework;

public class SafeDictionaryTest {
    [Test]
    public void Test1() {
        SafeDictionary<int, int> dict = new SafeDictionary<int, int>();
        dict[1] = 1;
        dict[2] = 2;
        dict[3] = 3;
        dict[4] = 4;
        int sum = 0;
        foreach (var (key, value) in dict) {
            sum += value;
            dict.Remove(key + 2);
        }
        Assert.AreEqual(sum, 3);
    }
    
    
    [Test]
    public void Test2() {
        Assert.Throws<ArgumentException>(() => {
            SafeDictionary<int, int> dict = new SafeDictionary<int, int>();
            dict.Add(1, 1);
            dict.Add(1, 2);
        });
    }
    
    [Test]
    public void Test3() {
        SafeDictionary<int, int> dict = new SafeDictionary<int, int>();
        dict.Add(1, 1);
        dict.Remove(1);
        dict.Add(1, 2);
    }
    
    [Test]
    public void Test4() {
        Assert.Throws<InvalidOperationException>(() => {
            SafeDictionary<int, int> dict = new SafeDictionary<int, int>();
            dict[1] = 1;
            foreach (var (key, value) in dict) {
                dict[key + 1] = value + 1;
            }
        });
    }
}