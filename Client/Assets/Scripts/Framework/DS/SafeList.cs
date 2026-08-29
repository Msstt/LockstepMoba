// 迭代安全的列表

using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework {
    public class SafeList<T> {
        private class Item {
            public T value;
            public bool isDeleted;
        }
        
        private Dictionary<T, Item> dict = new Dictionary<T, Item>();
        private Queue<Item> q1 = new Queue<Item>();
        private Queue<Item> q2 = new Queue<Item>();
        private int iterationId;
        private int activeIterationId;
        
        public SafeList() { }
        
        public SafeList(IEnumerable<T> collection) {
            foreach (var value in collection) {
                Add(value);
            }
        }
        
        public void Add(T value) {
            if (dict.TryGetValue(value, out Item item)) {
                item.isDeleted = false;
            } else {
                Item newItem = new Item {
                    value = value,
                    isDeleted = false,
                };
                dict[value] = newItem;
                q1.Enqueue(newItem);
            }
        }

        public void Remove(T value) {
            if (dict.TryGetValue(value, out var item)) {
                item.isDeleted = true;
            }
        }

        public Enumerator GetEnumerator() {
            if (activeIterationId != 0) {
                throw new InvalidOperationException("Cannot iterate multiple times at the same time");
            }
            unchecked {
                iterationId++;
                if (iterationId == 0) {
                    iterationId++;
                }
            }
            activeIterationId = iterationId;
            return new Enumerator(this, activeIterationId);
        }

        private void EndIteration(int id) {
            if (activeIterationId != id) {
                return;
            }
            while (q1.Any()) {
                Item item = q1.Dequeue();
                if (item.isDeleted) {
                    dict.Remove(item.value);
                } else {
                    q2.Enqueue(item);
                }
            }
            (q1, q2) = (q2, q1);
            activeIterationId = 0;
        }
        
        public struct Enumerator : IDisposable {
            private readonly SafeList<T> self;
            private readonly int iterationId;
            private int count;
            private bool isDisposed;
            public T Current { get; private set; }

            internal Enumerator(SafeList<T> self, int iterationId) {
                this.self = self;
                this.iterationId = iterationId;
                count = 0;
                isDisposed = false;
                Current = default;
            }
            
            public bool MoveNext() {
                if (isDisposed || self.activeIterationId != iterationId) {
                    return false;
                }
                while (self.q1.Any()) {
                    if (count++ >= 1000000) {
                        throw new InvalidOperationException("Too many modifications during enumeration");
                    } 
                    Item item = self.q1.Dequeue();
                    if (item.isDeleted) {
                        self.dict.Remove(item.value);
                    } else {
                        Current = item.value;
                        self.q2.Enqueue(item);
                        return true;
                    }
                }
                Dispose();
                return false;
            }

            public void Dispose() {
                if (isDisposed) {
                    return;
                }
                isDisposed = true;
                self.EndIteration(iterationId);
            }
        }
        
        #region 其他方法

        public int Count {
            get {
                int count = 0;
                foreach (var _ in this) {
                    count++;
                }
                return count;
            }
        }
        
        public T First() {
            foreach (var value in this) {
                return value;
            }
            return default;
        }
        
        #endregion
    }
}
