// 迭代安全的字典

using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework {
    public class SafeDictionary<K, T> {
        private class Pair {
            public K key;
            public T value;
            public bool isDeleted;
        }
        
        private Dictionary<K, Pair> dict = new Dictionary<K, Pair>();
        private Queue<Pair> q1 = new Queue<Pair>();
        private Queue<Pair> q2 = new Queue<Pair>();
        private bool isIterating = false;
        
        public SafeDictionary() { }
        
        public SafeDictionary(IEnumerable<(K, T)> collection) {
            foreach (var (key, value) in collection) {
                Add(key, value);
            }
        }
        
        public void Add(K key, T value) {
            if (dict.TryGetValue(key, out Pair pair)) {
                if (!pair.isDeleted && !EqualityComparer<T>.Default.Equals(pair.value, value)) {
                    throw new System.ArgumentException($"Key already exists with a different value: {key}");
                }
                pair.value = value;
                pair.isDeleted = false;
            } else {
                Pair newPair = new Pair {
                    key = key,
                    value = value,
                    isDeleted = false,
                };
                dict[key] = newPair;
                q1.Enqueue(newPair);
            }
        }

        public void Remove(K key) {
            if (dict.TryGetValue(key, out var pair)) {
                pair.isDeleted = true;
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
            private readonly SafeDictionary<K, T> self;
            private int count;
            public (K, T) Current { get; private set; }

            internal Enumerator(SafeDictionary<K, T> self) {
                this.self = self;
                count = 0;
                Current = default;
            }
            
            public bool MoveNext() {
                while (self.q1.Any()) {
                    if (count++ >= 1000000) {
                        throw new InvalidOperationException("Too many modifications during enumeration");
                    } 
                    Pair pair = self.q1.Dequeue();
                    if (pair.isDeleted) {
                        self.dict.Remove(pair.key);
                    } else {
                        Current = (pair.key, pair.value);
                        self.q2.Enqueue(pair);
                        return true;
                    }
                }
                (self.q1, self.q2) = (self.q2, self.q1);
                self.isIterating = false;
                return false;
            }
        }
        
        public bool ContainsKey(K key) {
            return dict.TryGetValue(key, out var pair) && !pair.isDeleted;
        }
        
        public T this[K key] {
            get => ContainsKey(key) ? dict[key].value : default;
            set {
                if (ContainsKey(key)) {
                    dict[key].value = value;
                } else {
                    Add(key, value);
                }
            }
        }
    }
}