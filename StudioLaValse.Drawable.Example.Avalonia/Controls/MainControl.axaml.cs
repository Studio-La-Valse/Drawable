using Avalonia;
using Avalonia.Controls;
using StudioLaValse.Drawable.Avalonia.Painters;
using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Geometry;
using StudioLaValse.Key;
using System;
using Avalonia.ReactiveUI;
using StudioLaValse.Drawable.Example.Avalonia.ViewModels;

namespace StudioLaValse.Drawable.Example.Avalonia.Controls;

public partial class MainControl : ReactiveUserControl<CanvasViewModel>, IObserver<InvalidationRequest<PersistentElement>>
{
    public static readonly DirectProperty<MainControl, SceneManager<PersistentElement, ElementId>> SceneManagerProperty = AvaloniaProperty
        .RegisterDirect<MainControl, SceneManager<PersistentElement, ElementId>>(nameof(SceneManager), o => o.SceneManager, (o, v) => o.SceneManager = v);

    public static readonly DirectProperty<MainControl, IObservable<BoundingBox>> SelectionBorderProperty = AvaloniaProperty
        .RegisterDirect<MainControl, IObservable<BoundingBox>>(nameof(SelectionBorder), o => o.SelectionBorder, (o, v) => o.SelectionBorder = v);

    public static readonly DirectProperty<MainControl, bool> EnablePanProperty = AvaloniaProperty
        .RegisterDirect<MainControl, bool>(nameof(EnablePan), o => o.EnablePan, (o, v) => o.EnablePan = v);

    public static readonly DirectProperty<MainControl, INotifyEntityChanged<PersistentElement>> InvalidatorProperty = AvaloniaProperty
        .RegisterDirect<MainControl, INotifyEntityChanged<PersistentElement>>(nameof(Invalidator), o => o.Invalidator, (o, v) => o.Invalidator = v);

    public static readonly DirectProperty<MainControl, IPipe> PipeProperty = AvaloniaProperty
        .RegisterDirect<MainControl, IPipe>(nameof(Pipe), o => o.Pipe, (o, v) => o.Pipe = v);


    private SceneManager<PersistentElement, ElementId> sceneManager;
    public SceneManager<PersistentElement, ElementId> SceneManager
    {
        get => sceneManager;
        set 
        {
            SetAndRaise(SceneManagerProperty, ref sceneManager, value);
            value?.Rerender(BaseBitmapPainter);
        }
    }
    private IObservable<BoundingBox> _selectionBorder;
    public IObservable<BoundingBox> SelectionBorder
    {
        get => _selectionBorder;
        set 
        {
            SetAndRaise(SelectionBorderProperty, ref _selectionBorder, value);
            SelectionBorderDisposable?.Dispose();
            SelectionBorderDisposable = value?.Subscribe(selectionBorder.CreateObserver(InteractiveCanvas));
        }
    }

    private bool enablePan;
    public bool EnablePan
    {
        get => enablePan;
        set 
        {
            SetAndRaise(EnablePanProperty, ref enablePan, value);
            PanEnabledDisposable?.Dispose();
            ZoomEnabledDisposable?.Dispose();

            if (value)
            {
                PanEnabledDisposable = InteractiveCanvas.EnablePan();
                ZoomEnabledDisposable = InteractiveCanvas.EnableZoom();
            }
        }
    }

    private INotifyEntityChanged<PersistentElement> invalidator;
    public INotifyEntityChanged<PersistentElement> Invalidator
    {
        get => invalidator;
        set 
        { 
            SetAndRaise(InvalidatorProperty, ref invalidator, value);
            InvalidatorDisposable?.Dispose();
            InvalidatorDisposable = value?.Subscribe(this);
        }
    }
    private IPipe pipe;
    public IPipe Pipe
    {
        get => pipe;
        set 
        {
            SetAndRaise(PipeProperty, ref pipe, value);
            PipeDisposable?.Dispose();
            if (value is not null)
            {
                PipeDisposable = InteractiveCanvas.Subscribe(value);
            }
        } 
    }



    public IDisposable? PanEnabledDisposable { get; set; }
    public IDisposable? ZoomEnabledDisposable { get; set; }
    public IDisposable? InvalidatorDisposable { get; set; }
    public IDisposable? SelectionBorderDisposable { get; set; }
    public IDisposable? PipeDisposable { get; set; }
    public BaseBitmapPainter BaseBitmapPainter { get; }
    public IInteractiveCanvas InteractiveCanvas => canvas;



    public MainControl()
    {
        InitializeComponent();

        BaseBitmapPainter = new GraphicsPainter(canvas);

        PanEnabledDisposable = canvas.EnablePan();
        ZoomEnabledDisposable = canvas.EnableZoom();
    }



    public void OnCompleted()
    {
        if (SceneManager is null)
        {
            throw new Exception("No scenemanager active to invalidate this element.");
        }

        SceneManager.RenderChanges(BaseBitmapPainter);
    }
    public void OnError(Exception error)
    {
        throw error;
    }
    public void OnNext(InvalidationRequest<PersistentElement> value)
    {
        if (SceneManager is null)
        {
            throw new Exception("No scenemanager active to invalidate this element.");
        }

        SceneManager.AddToQueue(value);
    }
}
