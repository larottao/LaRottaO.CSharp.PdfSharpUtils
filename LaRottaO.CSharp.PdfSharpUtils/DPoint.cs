using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaRottaO.CSharp.PdfSharpUtils
{
    public class DPoint
    {
        public double x { get; set; }
        public double y { get; set; }

        public DPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
}