using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.Interaction.Private;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Drawable.Interaction.UserInput;

namespace StudioLaValse.Drawable.Interaction.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="IBehavior"/> interface.
    /// </summary>
    public static class BehaviorExtensions
    {
        /// <summary>
        /// Extends the specified <see cref="IBehavior"/> with an interaction behavior that intercepts key pressed state and modifies the specified <see cref="ISelectionManager{TEntity}"/>'s behaviour accordingly. The modified <see cref="ISelectionManager{TEntity}"/> is passed through the out keyword.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="selectionManager"></param>
        /// <param name="intercepted"></param>
        /// <returns></returns>
        public static IBehavior InterceptKeys<TEntity>(this IBehavior pipe, ISelectionManager<TEntity> selectionManager, out ISelectionManager<TEntity> intercepted) where TEntity : class
        {
            var _pipe = new PipeInterceptKeys<TEntity>(pipe);
            intercepted = new SelectionWithKeyResponse<TEntity>(selectionManager, () => _pipe.ShiftPressed, () => _pipe.CtrlPressed);
            return _pipe;
        }

        /// <summary>
        /// Extends the specifief <see cref="IBehavior"/> with a special interaction behavior that allows simple mouse interaction.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="inputDispatcher"></param>
        /// <param name="scene"></param>
        /// <param name="entityChanged"></param>
        /// <returns></returns>
        public static IBehavior ThenHandleDefaultMouseInteraction<TEntity>(this IBehavior inputDispatcher, IEnumerable<BaseVisualParent<TEntity>> scene, INotifyEntityChanged<TEntity> entityChanged) where TEntity : class
        {
            return new PipeMouseInteraction<TEntity>(inputDispatcher, scene, entityChanged);
        }

        /// <summary>
        /// Extends the specifief <see cref="IBehavior"/> with a special interaction behavior that allows simple mouse hover effects.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="inputDispatcher"></param>
        /// <param name="scene"></param>
        /// <param name="entityChanged"></param>
        /// <returns></returns>
        public static IBehavior ThenHandleMouseHover<TEntity>(this IBehavior inputDispatcher, IEnumerable<BaseVisualParent<TEntity>> scene, INotifyEntityChanged<TEntity> entityChanged) where TEntity : class
        {
            return new PipeMouseHover<TEntity>(inputDispatcher, scene, entityChanged);
        }

        /// <summary>
        ///  Extends the specifief <see cref="IBehavior"/> with a special interaction behavior that allows simple mouse click events.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="inputDispatcher"></param>
        /// <param name="scene"></param>
        /// <param name="selectionManager"></param>
        /// <returns></returns>
        public static IBehavior ThenHandleDefaultClick<TEntity>(this IBehavior inputDispatcher, IEnumerable<BaseVisualParent<TEntity>> scene, ISelectionManager<TEntity> selectionManager) where TEntity : class
        {
            return new PipeSelection<TEntity>(inputDispatcher, scene, selectionManager);
        }

        /// <summary>
        /// Extends the specifief <see cref="IBehavior"/> with a special interaction behavior that allows a drag selection border to select elements. The selection border is represented by the <see cref="ObservableBoundingBox"/>.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="inputDispatcher"></param>
        /// <param name="scene"></param>
        /// <param name="selectionManager"></param>
        /// <param name="observable"></param>
        /// <param name="notifyEntityChanged"></param>
        /// <returns></returns>
        public static IBehavior ThenHandleSelectionBorder<TEntity>(this IBehavior inputDispatcher, IEnumerable<BaseVisualParent<TEntity>> scene, ISelectionManager<TEntity> selectionManager, ObservableBoundingBox observable, INotifyEntityChanged<TEntity> notifyEntityChanged) where TEntity : class
        {
            return new PipeSelectionBorder<TEntity>(inputDispatcher, scene, selectionManager, observable, notifyEntityChanged);
        }

        /// <summary>
        /// Extends the specifief <see cref="IBehavior"/> with a special interaction behavior that allows elements to be transformed by mouse events.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="inputDispatcher"></param>
        /// <param name="selectionManager"></param>
        /// <param name="scene"></param>
        /// <param name="notifyEntityChanged"></param>
        /// <returns></returns>
        public static IBehavior ThenHandleTransformations<TEntity>(this IBehavior inputDispatcher, ISelectionManager<TEntity> selectionManager, IEnumerable<BaseVisualParent<TEntity>> scene, INotifyEntityChanged<TEntity> notifyEntityChanged) where TEntity : class
        {
            return new PipeTransformations<TEntity>(inputDispatcher, selectionManager, scene, notifyEntityChanged);
        }

        /// <summary>
        /// Extends the specifief <see cref="IBehavior"/> with a special interaction behavior that rerenders changed elements after the previous behavior has been completed.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="inputDispatcher"></param>
        /// <param name="observable"></param>
        /// <returns></returns>
        public static IBehavior ThenRender<TEntity>(this IBehavior inputDispatcher, INotifyEntityChanged<TEntity> observable) where TEntity : class
        {
            return new PipeRerender<TEntity>(inputDispatcher, observable);
        }
    }
}
