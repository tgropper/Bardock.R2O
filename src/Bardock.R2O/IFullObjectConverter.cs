using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bardock.R2O
{
    public interface IFullObjectConverter
    {
        IEnumerable<Dictionary<string, object>> Convert(FullObjectConfiguration config);
    }
}
