using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaningTest.Buffer
{
    public interface ISharderProgram:IBuffer
    {
        uint Handle { get; }
    }
}
