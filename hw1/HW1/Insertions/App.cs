using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Insertions
{
    public class App
    {
        long inversions = 0;
        // my answer: 2407905288 

        public void Run()
        {
            String line;
            String filePath = @"C:\dev\algdesign\hw1\IntegerArray.txt";
            List<int> list = new List<int>();
            using (Stream stream = new FileStream(filePath, FileMode.Open)) {
                using (StreamReader reader = new StreamReader(stream)) { 
                    while ((line = reader.ReadLine()) != null) {
                        list.Add(int.Parse(line));
                    }
                }
            }
            int[] array = list.ToArray();
            Console.WriteLine("array length: " + array.Length);
            array = MergeSort(array);
            //PrintArray<int>(array);
            Console.WriteLine(inversions);
        }

        private int[] MergeSort(int[] array) {
            if (array == null || array.Length == 0 || array.Length == 1) {
                return array;
            }
            int middle = (0 + array.Length) / 2 - 1;
            int[] left = new int[middle + 1];
            for (int i = 0; i <= middle; i++)
			{
			    left[i] = array[i];
			}
           // Console.Write("left: ");
           // PrintArray<int>(left);
            
            int[] right = new int[array.Length - middle - 1];
            for (int i = middle + 1; i < array.Length; i++) {
                right[i - middle - 1] = array[i];
            }
            //Console.Write("right: ");
            //PrintArray<int>(right);

            left = MergeSort(left);
            right = MergeSort(right);
            int[] merged = Merge(left, right);
            //Console.Write("merged: ");
            //PrintArray<int>(merged);
            //Console.WriteLine();
            return merged;
        }

        private void PrintArray<T>(T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i] + " ");
            }
            Console.WriteLine();
        }

        private int[] Merge(int[] left, int[] right) {
            int i = 0;
            int j = 0;
            int k = 0;
            if (left == null) {
                return right;
            }
            else if (right == null) {
                return left;
            }
            int[] total = new int[left.Length + right.Length];
            //bool inverted = false;
            for (; i < left.Length || j < right.Length; ) {
                if (i < left.Length)
                {
                    if (j < right.Length)
                    {
                        if (left[i] < right[j])
                        {
                            total[k++] = left[i++];
                        }
                        else // right[j] < left[i]
                        {
                            inversions += (left.Length - i);
                            total[k++] = right[j++];
                        }
                    }
                    else
                    {
                        total[k++] = left[i++];
                    }
                }
                else {
                    if (j < right.Length) {
                        //inversions++;
                        total[k++] = right[j++];
                    }
                }
            }
            return total;
        }
        

    }
}
