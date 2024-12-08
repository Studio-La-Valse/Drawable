using StudioLaValse.Drawable.Avalonia.Controls;
using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Drawable.Interaction;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Drawable.WPF.ViewModels;
using StudioLaValse.Geometry;
using StudioLaValse.Key;
using System;
using System.Drawing;
using System.Windows;

namespace StudioLaValse.Drawable.Example.WPF.ViewModels
{
    public class CanvasViewModel : BaseViewModel
    {
        private readonly BaseBitmapPainter canvasPainter;

        public BaseBitmapPainter BaseBitmapPainter => canvasPainter;

        public IObservable<BaseDrawableElement> ElementEmitter
        {
            get => GetValue(() => ElementEmitter);
            set => SetValue(() => ElementEmitter, value);
        }
        public IInputObserver Pipe
        {
            get => GetValue(() => Pipe);
            set => SetValue(() => Pipe, value);
        }
        public IObservable<BoundingBox> SelectionBorder
        {
            get => GetValue(() => SelectionBorder);
            set => SetValue(() => SelectionBorder, value);
        }
        public double Zoom
        {
            get => GetValue(() => Zoom);
            set => SetValue(() => Zoom, value);
        }
        public double TranslateX
        {
            get => GetValue(() => TranslateX);
            set => SetValue(() => TranslateX, value);
        }
        public double TranslateY
        {
            get => GetValue(() => TranslateY);
            set => SetValue(() => TranslateY, value);
        }
        public Rect Bounds
        {
            get => GetValue(() => Bounds);
            set => SetValue(() => Bounds, value);
        }

        public bool EnablePan
        {
            get => GetValue(() => EnablePan);
            set => SetValue(() => EnablePan, value);
        }

        public bool EnableZoom
        {
            get => GetValue(() => EnableZoom);
            set => SetValue(() => EnableZoom, value);
        }

        public CanvasViewModel()
        {
            var emitter = new DrawableElementEmitter();
            canvasPainter = new DrawableElementEmitterBitmapPainter(emitter);

            ElementEmitter = emitter;

            Zoom = 1;

            Pipe = new BaseInputObserver();
        }

        public void CenterContent(BaseContentWrapper baseContentWrapper)
        {
            var boundingBox = baseContentWrapper.BoundingBox();

            if (boundingBox.Width == 0 || boundingBox.Height == 0)
            {
                return;
            }


            if (Bounds.Width == 0 || Bounds.Height == 0)
            {
                return;
            }

            // Step 0: calculate view center
            var viewCenterX = Bounds.Width / 2;
            var viewCenterY = Bounds.Height / 2;

            // Step 1: Calculate the current center points
            var contentCenterX = boundingBox.MinPoint.X + boundingBox.Width / 2.0;
            var contentCenterY = boundingBox.MinPoint.Y + boundingBox.Height / 2.0;

            // Step 2: Move content to center at (0,0)
            var initialTranslateX = -contentCenterX;
            var initialTranslateY = -contentCenterY;

            // Apply the initial translation to move the content to (0,0)
            TranslateX = initialTranslateX;
            TranslateY = initialTranslateY;

            // Step 3: Calculate the zoom factor to fit the content within the view
            var scaleX = Bounds.Width / boundingBox.Width;
            var scaleY = Bounds.Height / boundingBox.Height;
            var zoomFactor = Math.Min(scaleX, scaleY);

            // Apply the zoom factor
            Zoom = zoomFactor;

            // Move the content back to the center
            TranslateX += viewCenterX / zoomFactor;
            TranslateY += viewCenterY / zoomFactor;
        }
    }
}
