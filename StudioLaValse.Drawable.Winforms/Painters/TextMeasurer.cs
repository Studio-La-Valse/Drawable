using StudioLaValse.Drawable.Text;
using StudioLaValse.Drawable.Winforms.Extensions;
using StudioLaValse.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioLaValse.Drawable.Winforms.Painters;
public class TextMeasurer : IMeasureText
{
    public XY Measure(string text, FontFamilyCore fontFamily, double size)
    {
        var font = fontFamily.ToWindowsFont(size);
        var measuredSize = TextRenderer.MeasureText(text, font);
        var xy = new XY(measuredSize.Width, measuredSize.Height);
        return xy;
    }
}
