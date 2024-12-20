using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;
using System.Xml.Linq;

namespace StudioLaValse.Drawable.Interaction.Selection
{
    /// <summary>
    /// A selection manager that intercepts keyboard keys.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class SelectionWithKeyResponse<TEntity> : ISelectionManager<TEntity>, IInputObserver
    {
        private readonly ISelectionManager<TEntity> source;
        private bool shiftPressed;
        private bool controlPressed;


        internal SelectionWithKeyResponse(ISelectionManager<TEntity> source)
        {
            this.source = source;
        }

        /// <inheritdoc/>
        public bool Add(TEntity element)
        {
            if (controlPressed)
            {
                return source.Remove(element);
            }

            return source.Add(element);
        }

        /// <inheritdoc/>
        public bool AddRange(IEnumerable<TEntity> entities)
        {
            if (controlPressed)
            {
                return source.RemoveRange(entities);
            }

            return source.AddRange(entities);
        }

        /// <inheritdoc/>
        public bool Clear()
        {
            if (shiftPressed || controlPressed)
            {
                return false;
            }

            return source.Clear();
        }


        /// <inheritdoc/>
        public bool Remove(TEntity element)
        {
            if (shiftPressed || controlPressed)
            {
                return false;
            }

            return source.Remove(element);
        }

        /// <inheritdoc/>
        public bool RemoveRange(IEnumerable<TEntity> element)
        {
            if (shiftPressed || controlPressed)
            {
                return false;
            }

            return source.RemoveRange(element);
        }

        /// <inheritdoc/>
        public bool Set(TEntity element)
        {
            if (shiftPressed)
            {
                return source.Add(element);
            }

            if (controlPressed)
            {
                return source.Remove(element);
            }

            return source.Set(element);
        }

        /// <inheritdoc/>
        public bool SetRange(IEnumerable<TEntity> entities)
        {
            if (shiftPressed)
            {
                return source.AddRange(entities);
            }

            if (controlPressed)
            {
                return source.RemoveRange(entities);
            }

            return source.SetRange(entities);
        }


        /// <inheritdoc/>
        public IEnumerable<TEntity> GetSelection()
        {
            return source.GetSelection();
        }

        /// <inheritdoc/>
        public bool IsSelected(TEntity element)
        {
            return source.IsSelected(element);
        }

        /// <inheritdoc/>
        public bool HandleKeyDown(Key key)
        {
            if (key == Key.Shift)
            {
                shiftPressed = true;
            }

            if (key == Key.Control)
            {
                controlPressed = true;
            }

            return true;
        }

        /// <inheritdoc/>
        public bool HandleKeyUp(Key key)
        {
            if (key == Key.Shift)
            {
                shiftPressed = false;
            }

            if (key == Key.Control)
            {
                controlPressed = false;
            }

            if (key == Key.Escape)
            {
                Clear();
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public bool HandleLeftMouseButtonDown()
        {
            return true;
        }

        /// <inheritdoc/>
        public bool HandleLeftMouseButtonUp()
        {
            return true;
        }

        /// <inheritdoc/>
        public bool HandleMouseWheel(double delta)
        {
            return true;
        }

        /// <inheritdoc/>
        public bool HandleRightMouseButtonDown()
        {
            return true;
        }

        /// <inheritdoc/>
        public bool HandleRightMouseButtonUp()
        {
            return true;
        }

        /// <inheritdoc/>
        public bool HandleMouseMove(XY position)
        {
            return true;
        }
    }
}
