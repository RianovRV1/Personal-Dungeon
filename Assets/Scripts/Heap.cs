using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A heap that takes items of type T.
/// Key things to remember about a heap
/// Contents are just in an array of N size
/// Searching is LogN as we cut our search in half each time we check
/// Child node left is 2*Currentindex + 1
/// Child node right is 2*CurrentIndex + 2
/// Parent Node is (CurrentIndex - 1) / 2  Always rounded down 
/// </summary>
/// <typeparam name="T">Type of element in the Heap</typeparam>
public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex { get; set; }

}

public class Heap<T> where T : IHeapItem<T> {


    T[] items;
    int currentItemCount;
    public int Count { get { return currentItemCount; } }

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }
    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortAdd(item);
        currentItemCount++;
    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        sortRemove(items[0]);
        return firstItem;
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    

    void sortRemove(T Item) // sort performed on removing from an item from the heap, having a lower priority than the child means being swapped, implemented in ComparedTo method
    {
        while (true)
        {
            int childIndexLeft = Item.HeapIndex * 2 + 1;
            int childIndexRight = Item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < currentItemCount) //start with left child
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount)
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) //if right child has higher priority switch to right child 
                        swapIndex = childIndexRight;
                }

                if (Item.CompareTo(items[swapIndex]) < 0)
                    Swap(Item, items[swapIndex]);
                
                else
                    return;
            }
            else
                return;
        }
    }

    void SortAdd(T item) // sort used when adding a new element or when updating an existing item
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    public void UpdateItem(T item)
    {
        SortAdd(item);
    }

    void Swap(T itemA, T itemB) //this only works because C# passes by reference on objects in C++ we would need input pointers
    {
        //perform swap on nodes stored index
        int tempAIndex = itemA.HeapIndex; 
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = tempAIndex;
        //perform swap in array
        items[itemA.HeapIndex] = itemA;
        items[itemB.HeapIndex] = itemB;
    }
}

