using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HW4
{
    /// <summary>
    /// I tried implementing the iterative DFS, but for some reason that was wrong.
    /// DFS is the iterative version
    /// DFSRec is the recursive version
    /// DFS returned: 434821,968,459,314,211
    /// DFSRec returned: 434821,968,459,313,211 -- only one number is off, but this is the accepted answer
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Thread t = new Thread(Run, 20 * 1024 * 1024);
            t.Start();

            Console.WriteLine("done thread?");
            Console.ReadLine();
        }

        static void Run(object data) {
            SCCFinder finder = new SCCFinder();
            finder.Run();
        }
    }
}
