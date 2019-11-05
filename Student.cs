using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement
{
    public class Student
    {
        public int Sid { get; set; }

        [Required]

        [StringLength(255)]
        public String FirstName { get; set; }

        [StringLength(255)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]

        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        public String DateOfBirth { get; set; }
        [StringLength(2000)]
        public String Address { get; set; }
        [Required]

        [Range(1000000000, 9999999999)]
        public long PhoneNumber { get; set; }

        public String SCourse { get; set; }
        [Required]

        [DataType(DataType.Date)]

        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}",ApplyFormatInEditMode=true)]
        public String EnrolmentDate { get; set; }
    }
}
