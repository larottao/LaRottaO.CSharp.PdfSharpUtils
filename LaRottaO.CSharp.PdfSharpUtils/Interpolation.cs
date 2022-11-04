using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaRottaO.CSharp.PdfSharpUtils
{
    internal class Interpolation
    {
        public double linearInterpolation(double x0, double y0, double x1, double y1, double xd)
        {
            /*******************************************************************/
            //  Linear interpolation,
            //  as taught to me by the great Guido Lacorazza
            //
            //  x0          ------->    y0
            //  given x     ------->    what is y?
            //  x1          ------->    y1
            /*******************************************************************/

            return (y0 + ((y1 - y0) * ((xd - x0) / (x1 - x0))));
        }
    }
}