using System;
using System.ComponentModel.DataAnnotations;

namespace Common
{
    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }
        [Required(ErrorMessage ="the age must be specified")]
        [Range(18, 150)]
        public int? Age { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
