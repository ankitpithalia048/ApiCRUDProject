using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCRUDProject.ViewModel
{
    public class StudentViewModel
    {

        public int StudentID { get; set; }

        public string Name { get; set; }
        
        public string Subject { get; set; }
        public int SubjectID { get; set; }
    }
}
