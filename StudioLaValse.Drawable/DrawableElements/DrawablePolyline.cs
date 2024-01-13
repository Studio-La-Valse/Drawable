﻿using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.DrawableElements
{
    /// <summary>
    /// An implementation of the <see cref="BaseDrawableElement"/> that represents a drawable polyline.
    /// </summary>
    public class DrawablePolyline : BaseDrawableElement
    {
        public IEnumerable<XY> Points { get; }
        public ColorARGB Color { get; }
        public double StrokeWeight { get; }

        public DrawablePolyline(IEnumerable<XY> points, ColorARGB strokeColor, double strokeWeight)
        {
            Points = points;
            Color = strokeColor;
            StrokeWeight = strokeWeight;
        }

        public override BoundingBox GetBoundingBox()
        {
            var minX = 0d;
            var maxX = 0d;
            var minY = 0d;
            var maxY = 0d;

            var firstPoint = true;
            foreach (var point in Points)
            {
                if (firstPoint)
                {
                    minX = point.X;
                    maxX = point.X;
                    minY = point.Y;
                    maxY = point.Y;
                    firstPoint = false;
                    continue;
                }

                minX = Math.Min(minX, point.X);
                maxX = Math.Max(maxX, point.X);
                minY = Math.Min(minY, point.Y);
                maxY = Math.Max(maxY, point.Y);
            }

            return new BoundingBox(minX, maxX, minY, maxY);
        }
    }
}
