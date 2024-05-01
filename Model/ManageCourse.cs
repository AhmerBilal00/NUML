using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYPWeb.DataModel;
using FYPWeb.Model;

namespace FYPWeb.Model
{
    public class ManageCourse
    {
        public Coordinator coordinator { get; set; }
        public StudentsCours studentsCours { get; set; }
        public Result result { get; set; }
        public Student student { get; set; }
        public Cours Course { get; set; }
        public RepeatedCours RepeatedCours { get; set; }
        
        public List<Student> students { get; set; }
        public List<Cours> Courses { get; set; }
        public List<Programme> programmes { get; set; }

    }
}