using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    class Glass : PubItem
    {


        public override void PatronUsing()
        {
            IsAvailable = false;
        }

        public override void PatronLeaving()
        {
            IsDirty = true;
        }

        public void CleanGlass()
        {
            IsDirty = false;
            IsAvailable = true;
        }
    }
}
