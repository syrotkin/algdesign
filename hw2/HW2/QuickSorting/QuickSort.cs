using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QuickSorting
{
    public class QuickSort
    {
        private String fileName = @"C:\dev\algdesign\hw2\QuickSort.txt";

        int sum = 0;

        public void Run()
        {
            //int[] array = { 3, 8, 2, 5, 1, 4, 7, 6};          
            int[] array = getArray(); //{ 8, 7, 6, 5, 4, 3, 2, 1};
            //PrintArray(array);
            Sort(array, 0, array.Length - 1);
            PrintArray<int>(array);
            Console.WriteLine("sum = {0}", sum);
        }

       
        public void Sort(int[] array, int left, int right) {
            if (right - left <= 0) {
                return;
            }
            int size = right - left + 1;
            sum += size - 1;
            
            int pivotIndex = ChoosePivot(array, left, right);
            //PrintArray(array);
            //Console.WriteLine("left = {0}, right = {1}", left, right);
            //Console.WriteLine("size = {0}, sum = {1}", size, sum);
            pivotIndex = PartTim(array, left, right, pivotIndex);
            Sort(array, left, pivotIndex - 1);
            Sort(array, pivotIndex + 1, right);
        }

        private int RandBetween(int left, int right) {
            Random r= new Random(DateTime.Now.Millisecond);
            return r.Next(left, right + 1);
        }

        // returns the pivot index
        private int ChoosePivot(int[] array, int left, int right)
        {
            // case1;
            //return left; // 162085 //  
                        
            // case 2:
            //return right; //  164123

            /*
            // Case 3: choosing the median element of the following set: {array[left], array[mid], array[right]}
            // case 3: // 138382 
            int mid = left + (right - left) / 2;
            int a1 = array[left];
            int a2 = array[mid];
            int a3 = array[right];
            if ((a1 > a3 && a1 < a2) || (a1 > a2 && a1 < a3)) {
                return left;
            }
            else if ((a2 > a1 && a2 < a3) || (a2 > a3 && a2 < a1)) {
                return mid;
            }
            else {
                return right;
            }
            */

            // case 4: I got: 153056, 158303, 154026, 153755, 145380, 
            return RandBetween(left, right);
            

        }

        // from left to right _INCLUSIVE_
        private int PartTim(int[] array, int left, int right, int pivotIndex) {
            //Console.WriteLine("pivotindex = " + pivotIndex);
            if (pivotIndex != left) { // swap pivot and first element
                int temp2 = array[pivotIndex];
                array[pivotIndex] = array[left];
                array[left] = temp2;
                pivotIndex = left;
            }

            int pivot = array[pivotIndex];
            int i = left + 1;
            for (int j = left + 1; j <= right; j++)
            {
                if (array[j] < pivot) { // swap the newly seen element (less than the pivot) with the leftmost element that is greater than the pivot
                    int temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                    ++i;
                }   
            }
            // since the pivot is the 1st element, we need to do a final adjustment:
            // swap the pivot and the rightmost element less than the pivot
            int temp1 = array[i - 1];
            array[i - 1] = pivot;
            array[left] = temp1;

            return i - 1;
        }

        private int[] getArray()
        {
            String line;
            List<int> numbers = new List<int>();
            using (Stream stream = new FileStream(fileName, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        int nextNumber = int.Parse(line);
                        numbers.Add(nextNumber);
                    }
                }
            }
            return numbers.ToArray();
        }

        private void PrintArray<T>(T[] a)
        {
            for (int i = 0; i < a.Length; i++)
            {
                Console.Write(a[i] + " ");
            }
            Console.WriteLine();
        }
       
    }
}
