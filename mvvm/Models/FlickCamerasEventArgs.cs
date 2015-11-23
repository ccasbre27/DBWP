using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM
{
    class FlickCamerasEventArgs : EventArgs
    {
        public IList<Camera> Results { get; set; }

        public FlickCamerasEventArgs(IList<Camera> pResults)
        {
            Results = pResults;
        }
    }
}
