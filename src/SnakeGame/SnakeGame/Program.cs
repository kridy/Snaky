using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADIPlus.Drawing;

namespace SnakeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            using (AsciiGraphics g = AsciiGraphics.FromUnManagedConsole())
                g.DrawPoint(new AsciiPen('*', AsciiColors.Blue), new Point(10,10));

            Console.ReadKey(true);
        }
    }
}
