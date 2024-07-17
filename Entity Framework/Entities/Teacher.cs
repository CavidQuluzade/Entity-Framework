using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework.Entity
{
    internal class Teacher : Base
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public ICollection<Group> Groups { get; set; }
    }
}
