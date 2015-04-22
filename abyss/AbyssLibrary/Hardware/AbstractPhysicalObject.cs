using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public abstract class AbstractPhysicalObject : IPhysicalObject
    {
        public Guid ID { get; private set; }
        public string Name { get; set; }

        protected AbstractPhysicalObject()
            : this("Default")
        {
        }

        protected AbstractPhysicalObject(string name)
        {
            this.Name = name;
            this.ID = Guid.NewGuid();
        }
    }
}
