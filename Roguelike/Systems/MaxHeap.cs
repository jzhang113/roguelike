﻿using Roguelike.Interfaces;

namespace Roguelike.Systems
{
    class MaxHeap<T> where T : ISchedulable
    {
        private T[] _heap;
        private int _heapSize;
        private int[] _orderArray;
        private int _count;

        public MaxHeap() : this(10)
        {
        }

        public MaxHeap(int size)
        {
            _heap = new T[size];
            _orderArray = new int[size];
            _heapSize = 0;
            _count = 0;
        }

        public void Add(T item)
        {
            if (_heapSize >= _heap.Length)
                Resize();

            _orderArray[_heapSize] = _count++;
            _heap[_heapSize++] = item;
            ReheapUp();
        }

        public void Remove(T item)
        {
            for (int i = 0; i < _heapSize; i++)
            {
                if (_heap[i].Equals(item))
                {
                    _heap[i] = _heap[--_heapSize];
                    _orderArray[i] = _orderArray[_heapSize];
                    ReheapDown(i);
                    // Q: Can we just break here? Can actors appear multiple times on the queue?
                }
            }
        }

        public void UpdateAll()
        {
            for (int i = 0; i < _heapSize; i++)
            {
                _heap[i].Energy += _heap[i].RefreshRate;
            }
        }

        public T GetMax()
        {
            T item = _heap[0];
            _heap[0] = _heap[--_heapSize];
            _orderArray[0] = _orderArray[_heapSize];
            ReheapDown(0);

            return item;
        }

        public ISchedulable Peek() => _heap[0];
        public bool IsEmpty() => _heapSize == 0;
        public int Size() => _heapSize;

        private void ReheapUp()
        {
            int pos = _heapSize - 1;
            T oldItem = _heap[pos];
            int oldOrder = _orderArray[pos];

            while (pos > 0 && CompareItem(_heap[pos], _orderArray[pos], _heap[(pos - 1) / 2], _orderArray[(pos - 1) / 2]) > 0)
            {
                _orderArray[pos] = _orderArray[(pos - 1) / 2];
                _heap[pos] = _heap[(pos - 1) / 2];
                pos = (pos - 1) / 2;
            }

            _orderArray[pos] = oldOrder;
            _heap[pos] = oldItem;
        }

        private void ReheapDown(int initial)
        {
            int pos = initial;
            T oldItem = _heap[pos];
            int oldOrder = _orderArray[pos];

            while (pos < _heapSize)
            {
                int left = 2 * pos + 1;
                int right = 2 * pos + 2;
                int swap = pos;
                T tempItem = oldItem;
                int tempOrder = oldOrder;

                if (left < _heapSize && CompareItem(tempItem, tempOrder, _heap[left], _orderArray[left]) < 0)
                {
                    swap = left;
                    tempItem = _heap[left];
                    tempOrder = _orderArray[left];
                }

                if (right < _heapSize && CompareItem(tempItem, tempOrder, _heap[right], _orderArray[right]) < 0)
                    swap = right;

                if (swap == pos)
                {
                    break;
                }
                else
                {
                    _orderArray[pos] = _orderArray[swap];
                    _heap[pos] = _heap[swap];
                    pos = swap;
                }
            }

            _orderArray[pos] = oldOrder;
            _heap[pos] = oldItem;
        }

        private void Resize()
        {
            T[] newHeap = new T[_heap.Length * 2];
            int[] newOrder = new int[_heap.Length * 2];

            for (int i = 0; i < _heapSize; i++)
            {
                newOrder[i] = _orderArray[i];
                newHeap[i] = _heap[i];
            }
            
            _orderArray = newOrder;
            _heap = newHeap;
        }

        private int CompareItem(T a, int orderA, T b, int orderB)
        {
            int result = a.CompareTo(b);
            if (result != 0)
                return result;
            else
                return orderB - orderA;
        }
    }
}
