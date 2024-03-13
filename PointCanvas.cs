using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PathMaker
{
    public class PointCanvas : Control
    {
        private List<PathPoint> points = new List<PathPoint>();

        internal void AddPoint(PathPoint point)
        {
            points.Add(point);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            foreach (var point in points)
            {
                e.Graphics.FillEllipse(Brushes.Black, point.X, point.Y, 5, 5);
            }
        }
    }
}

