using StudioLaValse.Key;

namespace Example.Model
{
    public class BaseGhost : PersistentElement
    {
        public BaseGhost(IKeyGenerator<int> keyGenerator) : base(keyGenerator)
        {

        }
    }
}
