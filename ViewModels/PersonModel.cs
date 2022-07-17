using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonCSV.ViewModels
{
    public class PersonModel
    {
         public int Identity { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public Char Sex { get; set; }
        public string Mobile { get; set; }
        public bool Active { get; set; }
    }
}