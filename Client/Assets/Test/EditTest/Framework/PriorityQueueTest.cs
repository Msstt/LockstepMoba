using System;
using System.Collections.Generic;
using NUnit.Framework;
using Framework;

public class PriorityQueueTest {
    [Test]
    public void PriorityQueueTest1() {
        var queue = new PriorityQueue<string, int>();
        queue.Enqueue("2", 2);
        queue.Enqueue("4", 4);
        queue.Enqueue("5", 5);
        queue.Enqueue("3", 3);
        queue.Enqueue("1", 1);

        string element;
        queue.Dequeue(out element, out _);
        Assert.AreEqual(element, "1");
        queue.Dequeue(out element, out _);
        Assert.AreEqual(element, "2");
        queue.Dequeue(out element, out _);
        Assert.AreEqual(element, "3");
        queue.Dequeue(out element, out _);
        Assert.AreEqual(element, "4");
        queue.Dequeue(out element, out _);
        Assert.AreEqual(element, "5");
    }
    
    [Test]
    public void PriorityQueueTest2() {
        var queue = new PriorityQueue<string, int>(Comparer<int>.Create((x, y) => y - x));
        queue.Enqueue("1", 1);
        queue.Enqueue("2", 2);

        string element;
        queue.Dequeue(out element, out _);
        Assert.AreEqual(element, "2");
        queue.Dequeue(out element, out _);
        Assert.AreEqual(element, "1");
    }
    
    [Test]
    public void PriorityQueueTest3() {
        var queue = new PriorityQueue<string, int>();
        queue.Enqueue("1", 1);
        queue.Dequeue(out _, out _);
        Assert.Throws<InvalidOperationException>(() => queue.Dequeue(out _, out _));
    }
}