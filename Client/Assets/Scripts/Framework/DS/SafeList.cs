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
        private bool isIterating = false;
        
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
            if (isIterating) {
                throw new InvalidOperationException("Cannot iterate multiple times at the same time");
            }
            isIterating = true;
            return new Enumerator(this);
        }
        
        public struct Enumerator {
            private readonly SafeList<T> self;
            private int count;
            public T Current { get; private set; }

            internal Enumerator(SafeList<T> self) {
                this.self = self;
                count = 0;
                Current = default;
            }
            
            public bool MoveNext() {
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
                (self.q1, self.q2) = (self.q2, self.q1);
                self.isIterating = false;
                return false;
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