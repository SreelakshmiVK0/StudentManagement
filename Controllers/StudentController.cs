using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace StudentManagement.Controllers
{
    [ApiController]
    public class StudentController : ControllerBase
    {

        
        private static List<Courses> _courses =new List<Courses>();
        private static List<Student> _students = new List<Student>();

        public DateTimeStyles DataTimeStyle { get; private set; }
        public object DataTimeStyles { get; private set; }

        //post Api for Students
        [HttpPost("api/Students")]
        public  IActionResult AddStudent(Student student)
        {
            bool flag = false;
            DateTime date1, date2;
            var LastStudent = _students.OrderByDescending(x => x.Sid).FirstOrDefault();
            int id = LastStudent == null ? 1 : LastStudent.Sid + 1;
            foreach (var course in _courses)
            {
                if (student.SCourse==course.CourseName)
                {
                    flag = true;
                }

            }
            if(flag==false)
            {
                return Conflict("Course is not in the list");
            }
            if (DateTime.TryParseExact(student.DateOfBirth, new[] { "dd-MMM-yyyy" }, null, DateTimeStyles.None, out date1))
                String.Format("{0:dd-mmm-yyy}", date1);
            else
                return Conflict();
            if (DateTime.TryParseExact(student.EnrolmentDate, new[] { "dd-MMM-yyyy" }, null, DateTimeStyles.None, out date2))
                String.Format("{0:dd-mmm-yyy}", date2);
            else
                return Conflict();
            if (Convert.ToDateTime(student.DateOfBirth)>DateTime.Now)
            {
                return Conflict("enter a valid Date");
            }
            if (Convert.ToDateTime(student.EnrolmentDate) > DateTime.Now)
            {
                return Conflict("enter a valid Date");
            }

            var StudentToBeAdded = new Student
            {
                Sid=id,
                FirstName=student.FirstName,
                LastName=student.LastName,
                DateOfBirth=student.DateOfBirth,
                Address=student.Address,
                PhoneNumber=student.PhoneNumber,
                SCourse=student.SCourse,
                EnrolmentDate=student.EnrolmentDate

            };
            _students.Add(StudentToBeAdded);
            return Ok();
        }

        //POST Api for courses
        [HttpPost("api/Courses")]

        public IActionResult CreateCourse(Courses courses)
        {
            var lastCourse = _courses.OrderByDescending(x => x.Cid).FirstOrDefault();
            int id = lastCourse == null ? 1 : lastCourse.Cid + 1;

            var courseToBeAdded = new Courses
            {
                Cid = id,
                CourseName = courses.CourseName

            };
            _courses.Add(courseToBeAdded);
            return Ok();
        }

        //getApi for all Students
        [HttpGet("api/Students")]
        public IActionResult GetStudentDetails()
        {
            return Ok(_students);
        }
        //GET API for getting all the courses
        [HttpGet("api/Courses")]
        public IActionResult GetCourses()
        {
            return Ok(_courses);
        }

        //Get API for getting a particular student
        [HttpGet("api/Students/{id}")]
        public IActionResult GetStudentById(int id)
        {
            var student = _students.SingleOrDefault(x => x.Sid == id);
            if (student == null)
                return NotFound();
            return Ok(new Student 
            { 
                Sid=student.Sid,
                FirstName=student.FirstName,
                LastName=student.LastName,
                DateOfBirth=student.DateOfBirth,
                Address=student.Address,
                PhoneNumber=student.PhoneNumber,
                SCourse=student.SCourse,
                EnrolmentDate=student.EnrolmentDate

            });
        }
        //API TO GET STUDENTDETAILS ONLY
        [HttpGet("api/Students/List")]
        public IActionResult ListStudents()
        {

            var studentlist = _students.Select(x => new { x.FirstName, x.LastName });
            return Ok(studentlist);
        }

        //api for course details
        [HttpGet("api/Courses/Clist")]
        public IActionResult GetCourseDetails()
        {
            var courselist = _courses.GroupJoin(_students, x => x.CourseName, y => y.SCourse, (x, y) => new { coursename= x.CourseName,count=y.Count() });
            ///var courselist = coursedetails.GroupBy(x => x.CourseName).Select(x => new {coursename = x.Key, count = x.Count()});
            return Ok(courselist);
        }

        //GET API for getting a particular course
        [HttpGet("api/Courses/{id}")]
        public IActionResult GetCourseById(int id)
        {
            var course = _courses.SingleOrDefault(x => x.Cid == id);
            if (course == null)
                return NotFound();
            return Ok(new Courses
            {
                Cid=course.Cid,
                CourseName=course.CourseName
            });
        }

        //put api For student
        [HttpPut("api/Students/{id}")]
        public IActionResult UpdateStudent(int id,[FromBody] Student Ustudent)
        {
            bool flag = false;
            var student = _students.SingleOrDefault(x => x.Sid == id);
            if (student == null)
                return NotFound();
            foreach (var course in _courses)
            {
                if (Ustudent.SCourse == course.CourseName)
                {
                    flag = true;
                }

            }
            if (flag == false)
            {
                return Conflict("Course is not in the list");
            }
            if (Convert.ToDateTime(student.DateOfBirth) > DateTime.Now)
            {
                return Conflict("enter a valid Date");
            }
            if (Convert.ToDateTime(student.EnrolmentDate) > DateTime.Now)
            {
                return Conflict("enter a valid Date");
            }
            student.FirstName = Ustudent.FirstName;
            student.LastName = Ustudent.LastName;
            student.DateOfBirth = Ustudent.DateOfBirth;
            student.Address = Ustudent.Address;
            student.PhoneNumber = Ustudent.PhoneNumber;
            student.SCourse = Ustudent.SCourse;
            student.EnrolmentDate = student.EnrolmentDate;
            return Ok();
        }

        //put api FOR COURSES
        [HttpPut("api/Courses/{id}")]
        public IActionResult UpdateCourses(int id,[FromBody] Courses UCourse)
        {
            var course = _courses.SingleOrDefault(x => x.Cid == id);
            if (course == null)
                return NotFound();
            course.CourseName = UCourse.CourseName;
            return Ok();
        }

        //DELETE API Students
        [HttpDelete("api/Students/{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var student = _students.SingleOrDefault(x => x.Sid == id);
            if (student== null)
                return NotFound();
            _students.Remove(student);
            return Ok();

        }
        //Delete api courses
        [HttpDelete("api/Courses/{id}")]
        public IActionResult DeleteCourse(int id)
        {
            var course = _courses.SingleOrDefault(x => x.Cid == id);
            if(course==null)
               return NotFound();
            _courses.Remove(course);
            return Ok();
            
        }  
    }
}

