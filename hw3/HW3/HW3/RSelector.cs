using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW3
{
    public class RSelector
    {
        Random random;
        public void Run()
        {
            random = new Random();

            int[] array = { 3, 8, 2, 5, 1, 4, 7, 6 };
            PrintArray(array);
            Console.WriteLine();
            for (int i = 0; i < array.Length; i++)
            {
                //Console.WriteLine("pivot index = {0}, pivot = {1}", i, array[i]);
                int element = RSelect(array, 0, array.Length - 1, i);
                Console.WriteLine("{0}th largest element: {1}", i, element);
                Console.WriteLine();
                //array =  new int[] { 3, 8, 2, 5, 1, 4, 7, 6 };
            }
        }

        private void PrintArray(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i] + " ");
            }
            Console.WriteLine();
        }

        // find ith element of array.
        // low, high are low and high indices
        private int RSelect(int[] array, int low, int high, int i) {
            if (high <= low) {
                return array[low];
            }
            int pivotIndex = random.Next(low, high);
            int pivot = array[pivotIndex];
            int j = Partition(array, low, high, pivotIndex);
            // j is the new pivotIndex
            if (j == i)
            {
                return array[j];
            }
            else if (j > i)
            {  // recurse on the left of the array
                return RSelect(array, low, j - 1, i);
            }
            else { // j < i , recurse on the right of the array
                return RSelect(array, j + 1, high, i);
            }
        }
        
        private int Partition(int[] array, int low, int high, int pivotIndex)
        {
            int temp;
            if (pivotIndex != low)
            {
                // swap the 1st element with the chosen pivot. This is the setup of the algorithm. It requires the first element to be the pivot
                temp = array[pivotIndex];
                array[pivotIndex] = array[low];
                array[low] = temp;
            }
            int pivot = array[low];
            int i = low;
            int j = low + 1;
            for (; j <= high; j++)
            {
                if (array[j] > pivot) { 
                    //nothing
                }
                else if (array[j] < pivot) { 
                    // swap i + 1 and j
                    temp = array[i + 1];
                    array[i + 1] = array[j];
                    array[j] = temp;
                    i++;
                }
            }
            // swap pivot with largest element < p (i)
            temp = array[low];
            array[low] = array[i];
            array[i] = temp;
            return i; 
        }

    }
}
