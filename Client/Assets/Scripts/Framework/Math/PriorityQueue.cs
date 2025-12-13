using System;
using System.Collections.Generic;

namespace Framework {
    public class PriorityQueue<TElement, TPriority> {
        private List<Tuple<TElement, TPriority>> data = new List<Tuple<TElement, TPriority>>();
        private IComparer<TPriority> comparer;

        public int Count => data.Count;
        
        public PriorityQueue(IComparer<TPriority> comparer = null) {
            this.comparer = comparer ?? Comparer<TPriority>.Default;
        }
        
        public void Enqueue(TElement element, TPriority priority) {
            data.Add(Tuple.Create(element, priority));
            HeapUp(data.Count - 1);
        }

        public void Dequeue(out TElement element, out TPriority priority) {
            if (data.Count == 0) {
                throw new InvalidOperationException("PriorityQueue is empty");
            }
            
            (element, priority) = data[0];
            data[0] = data[data.Count - 1];
            data.RemoveAt(data.Count - 1);
            HeapDown(0);
        }
        
        private void HeapUp(int index) {
            while (index > 0) {
                int parent = (index - 1) / 2;
                if (comparer.Compare(data[index].Item2, data[parent].Item2) >= 0) {
                    break;
                }
                
                (data[index], data[parent]) = (data[parent], data[index]);
                index = parent;
            }
        }
        
        private void HeapDown(int index) {
            int count = data.Count;
            while (true) {
                int left = index * 2 + 1;
                int right = index * 2 + 2;
                int smallest = index;

                if (left < count && comparer.Compare(data[left].Item2, data[smallest].Item2) < 0) {
                    smallest = left;
                }
                if (right < count && comparer.Compare(data[right].Item2, data[smallest].Item2) < 0) {
                    smallest = right;
                }
                if (smallest == index) {
                    break;
                }
                (data[index], data[smallest]) = (data[smallest], data[index]);
                index = smallest;
            }
        }
    }
}