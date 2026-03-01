using System;
using Combat;
using NUnit.Framework;

public class PriorityTest {
    [Test]
    public void Test1() {
        Priority p1 = new Priority(10);
        p1.AddModifier(Priority.ModifierType.Add, 10);
        Assert.AreEqual(p1.Value, 20);
    }
    
    [Test]
    public void Test2() {
        Priority p1 = new Priority(10);
        p1.AddModifier(Priority.ModifierType.PercentAdd, 10);
        p1.AddModifier(Priority.ModifierType.PercentAdd, 10);
        Assert.AreEqual(p1.Value, 12);
    }
    
    [Test]
    public void Test3() {
        Priority p1 = new Priority(10);
        p1.AddModifier(Priority.ModifierType.PercentMult, 100);
        p1.AddModifier(Priority.ModifierType.PercentMult, 100);
        Assert.AreEqual(p1.Value, 40);
    }
    
    [Test]
    public void Test4() {
        Priority p1 = new Priority(10);
        p1.AddModifier(Priority.ModifierType.PercentMult, 100);
        p1.AddModifier(Priority.ModifierType.Add, 10);
        Assert.AreEqual(p1.Value, 40);
        p1.RemoveModifier(Priority.ModifierType.PercentMult, 100);
        Assert.AreEqual(p1.Value, 20);
    }
    
    [Test]
    public void Test5() {
        LimitedPriority p1 = new LimitedPriority(20);
        p1.AddModifier(Priority.ModifierType.Add, 20, LimitedPriority.ModifierType.Constant);
        Assert.AreEqual(p1.Value, 20);
        p1.AddModifier(Priority.ModifierType.Add, 40, LimitedPriority.ModifierType.PercentFollow);
        Assert.AreEqual(p1.Value, 40);
        p1.RemoveModifier(Priority.ModifierType.Add, 40, LimitedPriority.ModifierType.PercentFollow);
        p1.AddModifier(Priority.ModifierType.Add, -20, LimitedPriority.ModifierType.Follow);
        Assert.AreEqual(p1.Value, 0);
    }
}