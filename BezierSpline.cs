using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathMaker
{
    internal class BezierSpline
    {
        public List<QuinticBezier> beziers { get; } = new List<QuinticBezier>();

        public BezierSpline() { }

        public void AddBezier(QuinticBezier bezier)
        {
            beziers.Add(bezier);
        }

        public void RemoveBezier(QuinticBezier bezier)
        {
            beziers.Remove(bezier);
        }

        public List<PathPoint> Calculate()
        {
            if (beziers.Count == 0)
                return new List<PathPoint>();

            beziers[0].c0 = (beziers[0].k0 + beziers[0].c1) / 2.0;

            for (int i = 1; i < beziers.Count; ++i)
                beziers[i].c0 = (4.0 * beziers[i].k0 - beziers[i - 1].c2 + beziers[i].c0) / 4.0;

            for (int i = 0; i < beziers.Count - 1; ++i)
                beziers[i].c3 = 2.0 * beziers[i].k1 - beziers[i + 1].c0;

            beziers[beziers.Count - 1].c3 = (beziers[beziers.Count - 1].k1 + beziers[beziers.Count - 1].c2) / 2.0;

            List<PathPoint> path = new List<PathPoint>();
            foreach (var bezier in beziers)
            {
                for (var i = 0.0; i < 1.0; i += 0.02)
                {
                    path.Add(bezier.getPoint(i));
                }
            }
            return path;
        }
    }
}
