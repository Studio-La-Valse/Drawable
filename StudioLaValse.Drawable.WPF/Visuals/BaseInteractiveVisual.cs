using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;
using System.Reactive.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Key = System.Windows.Input.Key;

namespace StudioLaValse.Drawable.WPF.Visuals
{
    /// <summary>
    /// The base implementation for WPF <see cref="UserControl"/>s that need to implement <see cref="IInteractiveCanvas"/>.
    /// </summary>
    public abstract class BaseInteractiveVisual : UserControl, IInteractiveCanvas
    {
        public double Zoom { get; set; } = 1;
        public double TranslateX { get; set; } = 0;
        public double TranslateY { get; set; } = 0;
        public XY LastPosition { get; set; } = new XY(0, 0);

        public double ViewBoxWidth => double.IsNormal(Width) ? Width : 0;
        public double ViewBoxHeight => double.IsNormal(Height) ? Width : 0;

        new public IObservable<XY> MouseMove { get; }
        new public IObservable<bool> MouseLeftButtonDown { get; }
        new public IObservable<bool> MouseRightButtonDown { get; }
        new public IObservable<double> MouseWheel { get; }
        new public IObservable<Interaction.UserInput.Key> KeyDown { get; }
        new public IObservable<Interaction.UserInput.Key> KeyUp { get; }


        public BaseInteractiveVisual()
        {
            Focusable = true;

            XY getFromArgs(MouseEventArgs args)
            {
                var e = args.GetPosition(this);
                return this.HostToCanvas(new XY(e.X, e.Y));
            }

            MouseMove = Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(x => PreviewMouseMove += x, x => PreviewMouseMove -= x)
                .Select(e => e.EventArgs)
                .Select(getFromArgs);

            MouseLeftButtonDown = Observable.FromEventPattern<MouseButtonEventHandler, MouseButtonEventArgs>(x => PreviewMouseLeftButtonDown += x, x => PreviewMouseLeftButtonDown -= x)
                .Merge(Observable.FromEventPattern<MouseButtonEventHandler, MouseButtonEventArgs>(x => PreviewMouseLeftButtonUp += x, x => PreviewMouseLeftButtonUp -= x))
                .Select(e => e.EventArgs)
                .Where(e => e.ChangedButton == MouseButton.Left)
                .Select(e => e.LeftButton == MouseButtonState.Pressed);

            MouseRightButtonDown = Observable.FromEventPattern<MouseButtonEventHandler, MouseButtonEventArgs>(x => PreviewMouseRightButtonDown += x, x => PreviewMouseRightButtonDown -= x)
                .Merge(Observable.FromEventPattern<MouseButtonEventHandler, MouseButtonEventArgs>(x => PreviewMouseRightButtonUp += x, x => PreviewMouseRightButtonUp -= x))
                .Select(e => e.EventArgs)
                .Where(e => e.ChangedButton == MouseButton.Right)
                .Select(e => e.RightButton == MouseButtonState.Pressed);

            MouseWheel = Observable.FromEventPattern<MouseWheelEventHandler, MouseWheelEventArgs>(x => PreviewMouseWheel += x, x => PreviewMouseWheel -= x)
               .Select(e => e.EventArgs)
               .Select(e => e.Delta / 1000d);

            KeyDown = Observable.FromEventPattern<KeyEventHandler, KeyEventArgs>(x => PreviewKeyDown += x, x => PreviewKeyDown -= x)
                .Select(e => e.EventArgs)
                .Select(e => e.Key switch
                {
                    Key.LeftCtrl => Interaction.UserInput.Key.Control,
                    Key.LeftShift => Interaction.UserInput.Key.Shift,
                    Key.Escape => Interaction.UserInput.Key.Escape,
                    _ => Interaction.UserInput.Key.Unknown
                });

            KeyUp = Observable.FromEventPattern<KeyEventHandler, KeyEventArgs>(x => PreviewKeyUp += x, x => PreviewKeyUp -= x)
                .Select(e => e.EventArgs)
                .Select(e => e.Key switch
                {
                    Key.LeftCtrl => Interaction.UserInput.Key.Control,
                    Key.LeftShift => Interaction.UserInput.Key.Shift,
                    Key.Escape => Interaction.UserInput.Key.Escape,
                    _ => Interaction.UserInput.Key.Unknown
                });
        }



        public abstract void Refresh();
    }
}
