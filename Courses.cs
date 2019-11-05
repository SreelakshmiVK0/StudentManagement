using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement
{
    public class Courses
    {
        public int Cid { get; set; }
        [Required]
        public String CourseName { get; set; }
       
    }
}
