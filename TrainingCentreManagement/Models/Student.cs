using System;

namespace TrainingCentreManagement.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public int CourseId { get; set; }
    }
}