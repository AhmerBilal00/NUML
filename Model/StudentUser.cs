using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYPWeb.DataModel;

namespace FYPWeb.Model
{
    public class StudentUser
    {
        public Student Student { get; set; }
        public AdminStaff Admin { get; set; }
        public User User { get; set; }
        public UserRole UserRole { get; set; }
        public ExamBranch examBranch { get; set; }
        public Coordinator Coordinator { get; set; }
        public List<Campus> Campus { get; set; }
        public List<Cours> Course { get; set; }
        public Cours SingleCourse { get; set; }
        public Semester semester { get; set; }
        public List<Semester> semesters { get; set; }
        public List<Programme> Programmes { get; set; }
        public Programme Programme { get; set; }
        

    }
}