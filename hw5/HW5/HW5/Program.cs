﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW5
{
    class Program
    {
        static void Main(string[] args)
        {
            PathFinder finder = new PathFinder();
            finder.Run();

            Console.ReadKey();
        }
    }
}
