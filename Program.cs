using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
    class Program
    {
        static void Main(string[] args)
        {
            Matrix m = Matrix.RandomMatrix(500, 500, 10);
            DateTime begin = DateTime.Now;
            //Console.WriteLine(m.ToString());
            m.Invert();
            DateTime end = DateTime.Now;
            Console.WriteLine((end - begin).TotalSeconds);
        }
    }
}
