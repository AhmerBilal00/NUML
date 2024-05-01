using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FYPWeb.DataModel;
using FYPWeb.Model;
using Rotativa;

namespace FYPWeb.Controllers
{

    public class ExamBranchController : Controller
    {
        NUMLAutomatedTranscriptEntities db = new NUMLAutomatedTranscriptEntities();
        // GET: ExamBranch
        public ActionResult Dashboard()
        {
            if (Session["ExamBranch"] != null)
            {
                Exambranch Exb = new Exambranch
                {
                    exambranch = Session["ExamBranch"] as ExamBranch
                };
                
                return View(Exb);
            }
            return RedirectToAction("Index", "Student");
        }
        public ActionResult DashboardTest()
        {
            if (Session["ExamBranch"] != null)
            {
                Exambranch Exb = new Exambranch
                {
                    exambranch = Session["ExamBranch"] as ExamBranch
                };
                
                return View(Exb);
            }
            return RedirectToAction("Index", "Student");
        }
        public ActionResult PrintTranscript(int studentId)
            {
            if (studentId != 0)
            {
                Exambranch exambranch = new Exambranch();
                exambranch.student = db.Students.Where(x => x.StudentId == studentId).FirstOrDefault();
                exambranch.results = db.Results.Where(x => x.StudentId == studentId).OrderBy(x => x.SemesterId).ToList();

                exambranch.Courses = new List<Cours>();
                for (int i = 0; i < exambranch.results.Count; i++)
                {
                    var courseId = exambranch.results[i].CourseId; // Retrieve the ProgrammeId from Std.Course[i] outside of the LINQ query

                    Cours Course = (from c in db.Results
                                    join r in db.Courses on c.CourseId equals r.CourseId
                                    where r.CourseId == courseId // Use the retrieved ProgrammeId in the query
                                    select r).FirstOrDefault();


                    exambranch.Courses.Add(Course);
                }
                return View(exambranch);
            }
            return RedirectToAction("Index", "Student");
        }
        public ActionResult PrintAll(Exambranch Exb)
        {
            if (Session["ExamBranch"] != null)
            {
                var studentId = Exb.result.StudentId;
                return new ActionAsPdf("PrintTranscript", new { studentId });
            }
            return RedirectToAction("Index", "Student");

        }
    }
}