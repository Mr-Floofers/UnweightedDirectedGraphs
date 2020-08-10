using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace GraphsLibrary
{
    public class HeapTree<T>
    {
        T[] data;
        int Capacity;

        public int Count = 0;

        IComparer<T> comparer;

        public HeapTree(IComparer<T> comparer, int size = 4)
        {
            data = new T[size];
            Capacity = size;
            this.comparer = comparer;
        }

        public bool Insert(T value)
        {
            if(Contains(value))
            {
                return false;
            }
            if (Count == Capacity - 1)
            {
                Resize();
            }
            data[Count] = value;
            HeapifyUp(Count);
            Count++;
            return true;
        }

        public void HeapifyUp(int index)
        {
            while (index != 0 && comparer.Compare(data[ParentIndex(index)], data[index]) > 0)
            {
                Swap(ParentIndex(index), index);
                index = ParentIndex(index);
            }
        }

        public T Pop()
        {
            T val = data[0];
            Count--;
            Swap(0, Count);
            data[Count] = default;
            HeapifyDown(0);
            return val;
        }

        void HeapifyDown(int index)
        {
            int leftIndex;
            int rightIndex;
            int swapIndex;
            while (index < Count)
            {
                leftIndex = LeftIndex(index);
                rightIndex = RightIndex(index);
                if (leftIndex >= Count)
                {
                    return;
                }
                else if (rightIndex >= Count)
                {
                    if (comparer.Compare(data[leftIndex], data[index]) < 0)
                    {
                        swapIndex = leftIndex;
                    }
                    else
                    {
                        return;
                    }
                }
                else if (comparer.Compare(data[leftIndex], data[rightIndex]) < 0)
                {
                    swapIndex = leftIndex;
                }
                else
                {
                    swapIndex = rightIndex;
                }

                if (comparer.Compare(data[swapIndex], data[index]) < 0)
                {
                    Swap(swapIndex, index);
                    index = swapIndex;
                }
                else
                {
                    return;
                }
            }
        }

        int LeftIndex(int parentIndex)
        {
            return (parentIndex * 2) + 1;
        }
        int RightIndex(int parentIndex)
        {
            return (parentIndex * 2) + 2;
        }
        int ParentIndex(int childIndex)
        {
            return (childIndex - 1) / 2;
        }
        void Swap(int index1, int index2)
        {
            T tempVal = data[index1];
            data[index1] = data[index2];
            data[index2] = tempVal;
        }

        int Resize()
        {
            int newCapacity = Capacity * 2;
            T[] newData = new T[newCapacity];
            for (int i = 0; i < Capacity; i++)
            {
                newData[i] = data[i];
            }
            data = newData;
            Capacity = newCapacity;
            return Capacity;
        }

        public List<T> CreateSortedList()
        {
            int count = Count;
            List<T> returnData = new List<T>(count);

            for (int i = 0; i < count; i++)
            {
                returnData.Add(Pop());
            }
            return returnData;
        }

        public bool Contains(T value)
        {
            for(int i = 0; i < data.Length; i++)
            {
                if(comparer.Compare(data[i], value) == 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
