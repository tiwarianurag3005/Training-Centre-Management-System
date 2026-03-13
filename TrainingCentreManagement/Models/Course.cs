using System;

namespace TrainingCentreManagement.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        public string CourseName { get; set; }

        public string Duration { get; set; }

        public decimal Fee { get; set; }
    }
}