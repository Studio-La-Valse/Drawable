using StudioLaValse.Key;

namespace StudioLaValse.Drawable.Example.Model
{
    public class ComponentModel : PersistentElement
    {
        public PersistentElement Ghost { get; }

        public ComponentModel(IKeyGenerator<int> keyGenerator, PersistentElement ghost) : base(keyGenerator)
        {
            Ghost = ghost;
        }
    }
}
