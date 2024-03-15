using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace PathMaker
{
    public class PointCanvas : Control
    {
        private PathPoint? startPoint = null;
        private PathPoint? selectedPoint = null;
        private PathPoint? selectedPoint2 = null;
        private Point grabOffset = new Point();
        internal BezierSpline bezierSpline = new BezierSpline();

        private void SetPointFieldX(PathPoint point, double x)
        {
            point.X = (float)(x * (144 / Width));
        }

        private void SetPointFieldY(PathPoint point, double y)
        {
            point.Y = (float)((Height - y) * (144 / Width));
        }

        private double GetPointFieldX(PathPoint point)
        {
            return point.X * (Width / 144);
        }

        private double GetPointFieldY(PathPoint point)
        {
            return Height - (point.Y * (Height / 144));
        }
        private void AddBezier(QuinticBezier bezier)
        {
            bezierSpline.AddBezier(bezier);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            foreach (PathPoint point in bezierSpline.Calculate())
            {
                int radius = 2;
                e.Graphics.FillEllipse(Brushes.Green, point.X - radius, point.Y - radius, 2 * radius, 2 * radius);
            }

            if (bezierSpline.beziers.Count > 0)
            {
                int radius = 5;
                e.Graphics.FillEllipse(Brushes.Blue, bezierSpline.beziers[0].k0.X - radius, bezierSpline.beziers[0].k0.Y - radius, 2 * radius, 2 * radius);
            }
            else if (startPoint != null)
            {
                int radius = 5;
                e.Graphics.FillEllipse(Brushes.Blue, startPoint.X - radius, startPoint.Y - radius, 2 * radius, 2 * radius);
            }

            foreach (var bezier in bezierSpline.beziers)
            {
                int radius = 5;
                e.Graphics.FillEllipse(Brushes.Red, bezier.c1.X - radius, bezier.c1.Y - radius, 2 * radius, 2 * radius);
                e.Graphics.FillEllipse(Brushes.Red, bezier.c2.X - radius, bezier.c2.Y - radius, 2 * radius, 2 * radius);
                e.Graphics.FillEllipse(Brushes.Blue, bezier.k1.X - radius, bezier.k1.Y - radius, 2 * radius, 2 * radius);
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (selectedPoint != null)
                return;

            if (startPoint == null)
            {
                startPoint = new PathPoint(e.X, e.Y);
                Invalidate();
            }
            else if (bezierSpline.beziers.Count == 0)
            {
                PathPoint k0 = new PathPoint(startPoint.X, startPoint.Y);
                PathPoint k1 = new PathPoint(e.X, e.Y);
                PathPoint c0 = new PathPoint(k0.X + (1 * (k1.X - k0.X) / 5), k0.Y + (1 * (k1.Y - k0.Y) / 5));
                PathPoint c1 = new PathPoint(k0.X + (2 * (k1.X - k0.X) / 5), k0.Y + (2 * (k1.Y - k0.Y) / 5));
                PathPoint c2 = new PathPoint(k0.X + (3 * (k1.X - k0.X) / 5), k0.Y + (3 * (k1.Y - k0.Y) / 5));
                PathPoint c3 = new PathPoint(k0.X + (4 * (k1.X - k0.X) / 5), k0.Y + (4 * (k1.Y - k0.Y) / 5));
                QuinticBezier bezier = new QuinticBezier(k0, c0, c1, c2, c3, k1);
                AddBezier(bezier);
            }
            else
            {
                int lastIndex = bezierSpline.beziers.Count - 1;
                PathPoint k0 = new PathPoint(bezierSpline.beziers[lastIndex].k1.X, bezierSpline.beziers[lastIndex].k1.Y);
                PathPoint k1 = new PathPoint(e.X, e.Y);
                PathPoint c0 = new PathPoint(k0.X + (1 * (k1.X - k0.X) / 5), k0.Y + (1 * (k1.Y - k0.Y) / 5));
                PathPoint c1 = new PathPoint(k0.X + (2 * (k1.X - k0.X) / 5), k0.Y + (2 * (k1.Y - k0.Y) / 5));
                PathPoint c2 = new PathPoint(k0.X + (3 * (k1.X - k0.X) / 5), k0.Y + (3 * (k1.Y - k0.Y) / 5));
                PathPoint c3 = new PathPoint(k0.X + (4 * (k1.X - k0.X) / 5), k0.Y + (4 * (k1.Y - k0.Y) / 5));
                QuinticBezier bezier = new QuinticBezier(k0, c0, c1, c2, c3, k1);
                AddBezier(bezier);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            for (int i = 0; i < bezierSpline.beziers.Count; ++i)
            {
                QuinticBezier bezier = bezierSpline.beziers[i];
                if (Math.Sqrt(Math.Pow(bezier.k0.X - e.X, 2) + Math.Pow(bezier.k0.Y - e.Y, 2)) <= 5.0)
                {
                    selectedPoint = bezier.k0;
                    if (i != 0)
                        selectedPoint2 = bezierSpline.beziers[i - 1].k1;
                    grabOffset = new Point((int)(e.X - (selectedPoint.X + 5)), (int)(e.Y - (selectedPoint.Y + 5)));
                }
                else if (Math.Sqrt(Math.Pow(bezier.c1.X - e.X, 2) + Math.Pow(bezier.c1.Y - e.Y, 2)) <= 5.0)
                {
                    selectedPoint = bezier.c1;
                    grabOffset = new Point((int)(e.X - (selectedPoint.X + 5)), (int)(e.Y - (selectedPoint.Y + 5)));
                }
                else if (Math.Sqrt(Math.Pow(bezier.c2.X - e.X, 2) + Math.Pow(bezier.c2.Y - e.Y, 2)) <= 5.0)
                {
                    selectedPoint = bezier.c2;
                    grabOffset = new Point((int)(e.X - (selectedPoint.X + 5)), (int)(e.Y - (selectedPoint.Y + 5)));
                }
                else if (Math.Sqrt(Math.Pow(bezier.k1.X - e.X, 2) + Math.Pow(bezier.k1.Y - e.Y, 2)) <= 5.0)
                {
                    selectedPoint = bezier.k1;
                    if (i != bezierSpline.beziers.Count - 1)
                        selectedPoint2 = bezierSpline.beziers[i + 1].k0;
                    grabOffset = new Point((int)(e.X - (selectedPoint.X + 5)), (int)(e.Y - (selectedPoint.Y + 5)));
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (selectedPoint != null && e.Button == MouseButtons.Left)
            {
                Cursor = Cursors.Hand;
                int radius = 5;
                int x = e.X - grabOffset.X - radius;
                int y = e.Y - grabOffset.Y - radius;
                selectedPoint.X = x;
                selectedPoint.Y = y;
                if (selectedPoint2 != null)
                {
                    selectedPoint2.X = x;
                    selectedPoint2.Y = y;
                }
                bezierSpline.Calculate();
                Invalidate();
            }
            else
            {
                bool overPoint = false;
                foreach (QuinticBezier bezier in bezierSpline.beziers)
                {
                    if (Math.Sqrt(Math.Pow(bezier.k0.X - e.X, 2) + Math.Pow(bezier.k0.Y - e.Y, 2)) <= 5.0)
                    {
                        overPoint = true;
                        break;
                    }
                    else if (Math.Sqrt(Math.Pow(bezier.c1.X - e.X, 2) + Math.Pow(bezier.c1.Y - e.Y, 2)) <= 5.0)
                    {
                        overPoint = true;
                        break;
                    }
                    else if (Math.Sqrt(Math.Pow(bezier.c2.X - e.X, 2) + Math.Pow(bezier.c2.Y - e.Y, 2)) <= 5.0)
                    {
                        overPoint = true;
                        break;
                    }
                    else if (Math.Sqrt(Math.Pow(bezier.k1.X - e.X, 2) + Math.Pow(bezier.k1.Y - e.Y, 2)) <= 5.0)
                    {
                        overPoint = true;
                        break;
                    }
                }
                if (overPoint)
                    Cursor = Cursors.Hand;
                else
                    Cursor = Cursors.Default;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (selectedPoint != null)
                selectedPoint = null;
            if (selectedPoint2 != null)
                selectedPoint2 = null;
        }
    }
}

