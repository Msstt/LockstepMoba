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
        SafeDictionary<int, int> dict = new SafeDictionary<int, int>();
        dict[1] = 1;
        Assert.Throws<InvalidOperationException>(() => {
            foreach (var (key, value) in dict) {
                dict[key + 1] = value + 1;
            }
        });
        Assert.DoesNotThrow(() => {
            var enumerator = dict.GetEnumerator();
            enumerator.Dispose();
        });
    }

    [Test]
    public void BreakAppliesChangesAndDoesNotBlockNextEnumeration() {
        SafeDictionary<int, int> dict = new SafeDictionary<int, int>(new []{(1, 1), (2, 2), (3, 3)});

        foreach (var _ in dict) {
            dict.Remove(2);
            dict.Add(4, 4);
            break;
        }

        int sum = 0;
        foreach (var (_, value) in dict) {
            sum += value;
        }
        Assert.AreEqual(8, sum);
    }

    [Test]
    public void ExceptionDoesNotBlockNextEnumeration() {
        SafeDictionary<int, int> dict = new SafeDictionary<int, int>(new []{(1, 1), (2, 2), (3, 3)});

        Assert.Throws<InvalidOperationException>(() => {
            foreach (var _ in dict) {
                throw new InvalidOperationException();
            }
        });

        int sum = 0;
        foreach (var (_, value) in dict) {
            sum += value;
        }
        Assert.AreEqual(6, sum);
    }
}
