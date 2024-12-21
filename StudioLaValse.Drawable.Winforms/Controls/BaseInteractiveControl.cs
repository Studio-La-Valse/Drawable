using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;
using System.Reactive.Linq;

namespace StudioLaValse.Drawable.Winforms.Controls
{
    public abstract class BaseInteractiveControl : Panel, IInteractiveCanvas
    {
        public double Zoom { get; set; } = 1;
        public double TranslateX { get; set; } = 0;
        public double TranslateY { get; set; } = 0;
        public double ViewBoxWidth => double.IsNormal(Width) ? Width : 0;
        public double ViewBoxHeight => double.IsNormal(Height) ? Width : 0;


        new public IObservable<XY> MouseMove { get; }
        public IObservable<bool> MouseLeftButtonDown { get; }
        public IObservable<bool> MouseRightButtonDown { get; }
        new public IObservable<double> MouseWheel { get; }
        new public IObservable<Key> KeyDown { get; }
        new public IObservable<Key> KeyUp { get; }


        public BaseInteractiveControl()
        {
            Dock = DockStyle.Fill;
            DoubleBuffered = true;

            XY getFromArgs(MouseEventArgs args)
            {
                var e = args.Location;
                return this.HostToCanvas(new XY(e.X, e.Y));
            }

            MouseMove = Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(x => base.MouseMove += x, x => base.MouseMove -= x)
                .Select(e => e.EventArgs)
                .Select(getFromArgs);

            MouseLeftButtonDown = Observable
                .FromEventPattern<MouseEventHandler, MouseEventArgs>(x => MouseDown += x, x => MouseDown -= x)
                .Where(e => e.EventArgs.Button == MouseButtons.Left)
                .Select(e => true)
                .Merge(Observable
                    .FromEventPattern<MouseEventHandler, MouseEventArgs>(x => MouseUp += x, x => MouseUp -= x)
                    .Where(e => e.EventArgs.Button == MouseButtons.Left)
                    .Select(e => false));

            MouseRightButtonDown = Observable
                .FromEventPattern<MouseEventHandler, MouseEventArgs>(x => MouseDown += x, x => MouseDown -= x)
                .Where(e => e.EventArgs.Button == MouseButtons.Right)
                .Select(e => true)
                .Merge(Observable
                    .FromEventPattern<MouseEventHandler, MouseEventArgs>(x => MouseUp += x, x => MouseUp -= x)
                    .Where(e => e.EventArgs.Button == MouseButtons.Right)
                    .Select(e => false));

            MouseWheel = Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(x => base.MouseWheel += x, x => base.MouseWheel -= x)
               .Select(e => e.EventArgs)
               .Select(e => e.Delta / 1000d);

            KeyDown = Observable.FromEventPattern<KeyEventHandler, KeyEventArgs>(x => base.KeyDown += x, x => base.KeyDown -= x)
                .Select(e => e.EventArgs)
                .Select(e => e.KeyData switch
                {
                    Keys.LControlKey => Key.Control,
                    Keys.LShiftKey => Key.Shift,
                    Keys.Escape => Key.Escape,
                    _ => Key.Unknown
                });

            KeyUp = Observable.FromEventPattern<KeyEventHandler, KeyEventArgs>(x => base.KeyUp += x, x => base.KeyUp -= x)
                .Select(e => e.EventArgs)
                .Select(e => e.KeyData switch
                {
                    Keys.LControlKey => Key.Control,
                    Keys.LShiftKey => Key.Shift,
                    Keys.Escape => Key.Escape,
                    Keys.R => Key.R,
                    Keys.Z => Key.Z,
                    Keys.Delete => Key.Delete,
                    _ => Key.Unknown
                });
        }

        new public abstract void Refresh();
    }
}
