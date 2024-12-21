using StudioLaValse.Key;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Model;


public class TextModel : PersistentElement
{
    public TextModel(IKeyGenerator<int> keyGenerator) : base(keyGenerator)
    {

    }

    
}
