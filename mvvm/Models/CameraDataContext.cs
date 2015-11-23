using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Models
{
    class CameraDataContext : DataContext
    {
        public CameraDataContext(string pConexion) : base(pConexion)
        {
            
        }

        public Table<Camera> camaras;
    }
}
