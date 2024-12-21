using StudioLaValse.Key;

namespace Example
{
    public class IncrementalKeyGenerator : IKeyGenerator<int>
    {
        private int _counter = 0;
        public IncrementalKeyGenerator()
        {

        }
        public int Generate()
        {
            var value = _counter;
            _counter++;
            return value;
        }
    }
}
