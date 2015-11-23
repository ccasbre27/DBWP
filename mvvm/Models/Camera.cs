using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM
{
    [Table]
    public class Camera
    {
        [Column(IsDbGenerated=true,IsPrimaryKey=true)]
        public int Id { get; set; }
        [Column]
        public string Name { get; set; }
    }
}
