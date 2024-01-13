using StudioLaValse.Key;

namespace StudioLaValse.Drawable.Example.Model
{
    public class CurveScene : PersistentElement
    {
        public CurveScene(IKeyGenerator<int> keyGenerator, CurveModel curveModel) : base(keyGenerator)
        {
            CurveModel = curveModel;
        }

        public CurveModel CurveModel { get; }
    }
}
