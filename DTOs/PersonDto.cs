using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonCSV.DTOs
{
    public class PersonDto
    {
        public int Identity { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public char Sex { get; set; }
        public string Mobile { get; set; }
        public bool Active { get; set; }

    }

}