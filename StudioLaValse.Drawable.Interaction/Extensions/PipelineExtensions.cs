using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.Interaction.Private;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Drawable.Interaction.UserInput;

namespace StudioLaValse.Drawable.Interaction.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="IPipe"/> interface.
    /// </summary>
    public static class PipelineExtensions
    {
        /// <summary>
        /// Extends the specified <see cref="IPipe"/> with an interaction pipe that intercepts key pressed state and modifies the specified <see cref="ISelectionManager{TEntity}"/>'s behaviour accordingly. The modified <see cref="ISelectionManager{TEntity}"/> is passed through the out keyword.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pipe"></param>
        /// <param name="selectionManager"></param>
        /// <param name="intercepted"></param>
        /// <returns></returns>
        public static IPipe InterceptKeys<TEntity>(this IPipe pipe, ISelectionManager<TEntity> selectionManager, out ISelectionManager<TEntity> intercepted) where TEntity : class, IEquatable<TEntity>
        {
            var _pipe = new PipeInterceptKeys<TEntity>(pipe);
            intercepted = new SelectionWithKeyResponse<TEntity>(selectionManager, () => _pipe.ShiftPressed, () => _pipe.CtrlPressed);
            return _pipe;
        }

        /// <summary>
        /// Extends the specifief <see cref="IPipe"/> with a special interaction pipe that allows simple mouse interaction.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="inputDispatcher"></param>
        /// <param name="scene"></param>
        /// <param name="entityChanged"></param>
        /// <returns></returns>
        public static IPipe ThenHandleDefaultMouseInteraction<TEntity>(this IPipe inputDispatcher, IEnumerable<BaseVisualParent<TEntity>> scene, INotifyEntityChanged<TEntity> entityChanged) where TEntity : class, IEquatable<TEntity>
        {
            return new PipeMouseInteraction<TEntity>(inputDispatcher, scene, entityChanged);
        }

        /// <summary>
        /// Extends the specifief <see cref="IPipe"/> with a special interaction pipe that allows simple mouse hover effects.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="inputDispatcher"></param>
        /// <param name="scene"></param>
        /// <param name="entityChanged"></param>
        /// <returns></returns>
        public static IPipe ThenHandleMouseHover<TEntity>(this IPipe inputDispatcher, IEnumerable<BaseVisualParent<TEntity>> scene, INotifyEntityChanged<TEntity> entityChanged) where TEntity : class, IEquatable<TEntity>
        {
            return new PipeMouseHover<TEntity>(inputDispatcher, scene, entityChanged);
        }

        /// <summary>
        ///  Extends the specifief <see cref="IPipe"/> with a special interaction pipe that allows simple mouse click events.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="inputDispatcher"></param>
        /// <param name="scene"></param>
        /// <param name="selectionManager"></param>
        /// <returns></returns>
        public static IPipe ThenHandleDefaultClick<TEntity>(this IPipe inputDispatcher, IEnumerable<BaseVisualParent<TEntity>> scene, ISelectionManager<TEntity> selectionManager) where TEntity : class, IEquatable<TEntity>
        {
            return new PipeSelection<TEntity>(inputDispatcher, scene, selectionManager);
        }

        /// <summary>
        /// Extends the specifief <see cref="IPipe"/> with a special interaction pipe that allows a drag selection border to select elements. The selection border is represented by the <see cref="ObservableBoundingBox"/>.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="inputDispatcher"></param>
        /// <param name="scene"></param>
        /// <param name="selectionManager"></param>
        /// <param name="observable"></param>
        /// <param name="notifyEntityChanged"></param>
        /// <returns></returns>
        public static IPipe ThenHandleSelectionBorder<TEntity>(this IPipe inputDispatcher, IEnumerable<BaseVisualParent<TEntity>> scene, ISelectionManager<TEntity> selectionManager, ObservableBoundingBox observable, INotifyEntityChanged<TEntity> notifyEntityChanged) where TEntity : class, IEquatable<TEntity>
        {
            return new PipeSelectionBorder<TEntity>(inputDispatcher, scene, selectionManager, observable, notifyEntityChanged);
        }

        /// <summary>
        /// Extends the specifief <see cref="IPipe"/> with a special interaction pipe that allows elements to be transformed by mouse events.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="inputDispatcher"></param>
        /// <param name="selectionManager"></param>
        /// <param name="scene"></param>
        /// <param name="notifyEntityChanged"></param>
        /// <returns></returns>
        public static IPipe ThenHandleTransformations<TEntity>(this IPipe inputDispatcher, ISelectionManager<TEntity> selectionManager, IEnumerable<BaseVisualParent<TEntity>> scene, INotifyEntityChanged<TEntity> notifyEntityChanged) where TEntity : class, IEquatable<TEntity>
        {
            return new PipeTransformations<TEntity>(inputDispatcher, selectionManager, scene, notifyEntityChanged);
        }

        /// <summary>
        /// Extends the specifief <see cref="IPipe"/> with a special interaction pipe that rerenders changed elements after the previous pipe has been completed.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="inputDispatcher"></param>
        /// <param name="observable"></param>
        /// <returns></returns>
        public static IPipe ThenRender<TEntity>(this IPipe inputDispatcher, INotifyEntityChanged<TEntity> observable) where TEntity : class, IEquatable<TEntity>
        {
            return new PipeRerender<TEntity>(inputDispatcher, observable);
        }
    }
}
