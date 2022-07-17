using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonCSV.Models
{
    public class Person
    {
        public int Id { get; set; }
        public int Identity { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public string Mobile { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

    }

    enum Gender
    {
        m, f
    }
}