using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    abstract class PubItem
    {
        public bool IsAvailable { get; internal set; } = true;

        public virtual void PatronUsing()
        {
            IsAvailable = false;
        }

        public virtual void PatronLeaving()
        {
            IsAvailable = true;
        }
    }
}
