using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public interface ISubProcessor
    {
        Guid ID { get; }
        string Name { get; set; }
    }
}
