using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathMaker
{
    internal class QuinticBezier
    {
        public PathPoint k0 { get; set; }
        public PathPoint c0 { get; set; }
        public PathPoint c1 { get; set; }
        public PathPoint c2 { get; set; }
        public PathPoint c3 { get; set; }
        public PathPoint k1 { get; set; }

        public QuinticBezier(PathPoint k0, PathPoint c0, PathPoint c1, PathPoint c2, PathPoint c3, PathPoint k1)
        {
            this.k0 = k0;
            this.c0 = c0;
            this.c1 = c1;
            this.c2 = c2;
            this.c3 = c3;
            this.k1 = k1;
        }

        public PathPoint getPoint(double t)
        {
            return Math.Pow(1 - t, 5) * k0
                + 5 * Math.Pow(1 - t, 4) * t * c0
                + 10 * Math.Pow(1 - t, 3) * Math.Pow(t, 2) * c1
                + 10 * Math.Pow(1 - t, 2) * Math.Pow(t, 3) * c2
                + 5 * (1 - t) * Math.Pow(t, 4) * c3
                + Math.Pow(t, 5) * k1;
        }
    }
}
