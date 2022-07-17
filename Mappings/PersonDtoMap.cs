using CsvHelper.Configuration;
using PersonCSV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonCSV.Mappings
{
    public class PersonDtoMap : ClassMap<Person>
    {
        public PersonDtoMap()
        {
            Map(p => p.Identity).Index(0);
            Map(p => p.FirstName).Index(1);
            Map(p => p.Surname).Index(2);
            Map(p => p.Age).Index(3);
            Map(p => p.Sex).Index(4);
            Map(p => p.Mobile).Index(5);
            Map(p => p.Active).Index(6);

        }
    }
}