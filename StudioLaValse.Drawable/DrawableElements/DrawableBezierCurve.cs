using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.DrawableElements
{
    /// <summary>
    /// An drawable bezier curve as an extension of a drawable polyline.
    /// </summary>
    public class DrawableBezierCurve : DrawablePolyline
    {
        public DrawableBezierCurve(IEnumerable<XY> points, ColorARGB strokeColor, double strokeWeight) : base(points, strokeColor, strokeWeight)
        {

        }

        /** Find the ~closest point on a Bézier curve to a point you supply.
         * out    : A vector to modify to be the point on the curve
         * curve  : Array of vectors representing control points for a Bézier curve
         * pt     : The point (vector) you want to find out to be near
         * tmps   : Array of temporary vectors (reduces memory allocations)
         * returns: The parameter t representing the location of `out`
         */
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

        /** Find a minimum point for a bounded function. May be a local minimum.
         * minX   : the smallest input value
         * maxX   : the largest input value
         * ƒ      : a function that returns a value `y` given an `x`
         * ε      : how close in `x` the bounds must be before returning
         * returns: the `x` value that produces the smallest `y`
         */
        public double LocalMinimum(double minX, double maxX, Func<double, double> ƒ, double ε)
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

        /** Calculate a point along a Bézier segment for a given parameter.
         * out    : A vector to modify to be the point on the curve
         * curve  : Array of vectors representing control points for a Bézier curve
         * t      : Parameter [0,1] for how far along the curve the point should be
         * tmps   : Array of temporary vectors (reduces memory allocations)
         * returns: out (the vector that was modified)
         */
        public static XY BezierPoint(ICollection<XY> curve, double t)
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
