using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework.Entity
{
    internal class Group : Base
    {
        public string Name { get; set; }
        public int Limit { get; set; }
        public ICollection<Student> Students { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}
