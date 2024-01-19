using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.DrawableElements
{
    /// <summary>
    /// An drawable bezier curve as an extension of a drawable polyline.
    /// </summary>
    public class DrawableBezierCurve : DrawablePolyline
    {
        /// <summary>
        /// The primary constructor.
        /// </summary>
        /// <param name="points"></param>
        /// <param name="strokeColor"></param>
        /// <param name="strokeWeight"></param>
        public DrawableBezierCurve(IEnumerable<XY> points, ColorARGB strokeColor, double strokeWeight) : base(points, strokeColor, strokeWeight)
        {

        }

        /// <summary>
        /// Find the closest point on a Bezier curve.
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public XY ClosestPoint(XY pt)
        {
            var mindex = 0d;
            var scans = 25d; // More scans -> better chance of being correct
            var min = double.MaxValue;
            var best = new XY(0, 0);
            var points = Points.ToArray();
            for (var i = scans + 1; i >= 0; i--)
            {
                var t = (double)i / scans;
                var bp = BezierPoint(points, t);
                var d = pt.DistanceTo(bp);
                var d2 = d * d;
                if (d2 < min)
                {
                    best = bp;
                    min = d2;
                    mindex = i;
                }
            }
            var t0 = Math.Max((mindex - 1) / scans, 0);
            var t1 = Math.Min((mindex + 1) / scans, 1);
            double d2ForT(double t)
            {
                var dist = pt.DistanceTo(BezierPoint(points, t));
                return dist * dist;
            }
            var parm = LocalMinimum(t0, t1, d2ForT, 1e-4);
            return BezierPoint(points, parm);
        }


        private static double LocalMinimum(double minX, double maxX, Func<double, double> ƒ, double ε)
        {
            var m = minX;
            var n = maxX;
            var k = 0d;
            while (n - m > ε)
            {
                k = (n + m) / 2;
                if (ƒ(k - ε) < ƒ(k + ε)) n = k;
                else m = k;
            }
            return k;
        }

        private static XY BezierPoint(ICollection<XY> curve, double t)
        {
            if (curve.Count < 2) throw new Exception("At least 2 control points are required");

            var tmps = curve.ToArray();

            for (var degree = curve.Count - 1; degree >= 0; degree--)
            {
                for (var i = 0; i < degree; ++i)
                {
                    tmps[i] = Lerp(tmps[i], tmps[i + 1], t);
                }
            }

            return tmps[0];
        }

        private static XY Lerp(XY first, XY second, double t)
        {
            return new XY(first.X + (second.X - first.X) * t, first.Y + (second.Y - first.Y) * t);
        }
    }
}
