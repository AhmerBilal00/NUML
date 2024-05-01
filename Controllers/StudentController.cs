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
    public class StudentController : Controller
    {
        NUMLAutomatedTranscriptEntities db = new NUMLAutomatedTranscriptEntities();
        // GET: Student
        public ActionResult Index()
        {
            string message = TempData["User"] as string; // Retrieve message from TempData
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Error = message;
            }
            return View();
        }
        public ActionResult Dashboard()
        {
            if (Session["Student"] != null)
            {
                StdResult stdResult = new StdResult();
                stdResult.student = Session["Student"] as Student;
                stdResult.results = db.Results.Where(x=>x.StudentId==stdResult.student.StudentId).ToList();
                if(stdResult.results.Count!=0)
                {
                    var data = stdResult.results.OrderByDescending((X) => X.SemesterId).FirstOrDefault();
                    stdResult.Totalsemester =(int)data.SemesterId;
                }
                else
                {
                    stdResult.Totalsemester = 0;
                }
                return View(stdResult);
            }
            return RedirectToAction("Index", "Student");
        }
        public ActionResult STranscript()
        {
            return View();
        }
        public ActionResult Transcript()
        {
            if (Session["Student"] != null)
            {
                StdResult stdResult = new StdResult();
                stdResult.student = Session["Student"] as Student;
                stdResult.results = db.Results.Where(x=> x.StudentId == stdResult.student.StudentId).OrderBy(X=>X.SemesterId).ToList();
                stdResult.Course = new List<Cours>();
                for (int i = 0; i < stdResult.results.Count; i++)
                {
                    var courseId = stdResult.results[i].CourseId; // Retrieve the ProgrammeId from Std.Course[i] outside of the LINQ query

                    Cours Course = (from c in db.Results
                                    join r in db.Courses on c.CourseId equals r.CourseId
                                    where r.CourseId == courseId // Use the retrieved ProgrammeId in the query
                                    select r).FirstOrDefault();


                    stdResult.Course.Add(Course);
                }
                return View(stdResult);
            }
            return RedirectToAction("Index", "Student");
        }
        public ActionResult PrintTranscripts()
        {
            if (Session["Student"] != null)
            {
                StdResult stdResult = new StdResult();
                stdResult.student = Session["Student"] as Student;
                stdResult.results = db.Results.Where(x=> x.StudentId == stdResult.student.StudentId).OrderBy(X=>X.SemesterId).ToList();
                stdResult.Course = new List<Cours>();
                for (int i = 0; i < stdResult.results.Count; i++)
                {
                    var courseId = stdResult.results[i].CourseId; // Retrieve the ProgrammeId from Std.Course[i] outside of the LINQ query

                    Cours Course = (from c in db.Results
                                    join r in db.Courses on c.CourseId equals r.CourseId
                                    where r.CourseId == courseId // Use the retrieved ProgrammeId in the query
                                    select r).FirstOrDefault();


                    stdResult.Course.Add(Course);
                }
                return View(stdResult);
            }
            return RedirectToAction("Index", "Student");
        }
        public ActionResult PrintTranscript(int studentId)
        {
            if (studentId != 0)
            {
                StdResult stdResult = new StdResult();
                stdResult.student = db.Students.Where(x => x.StudentId == studentId).FirstOrDefault();
                stdResult.results = db.Results.Where(x => x.StudentId == stdResult.student.StudentId).OrderBy(X => X.SemesterId).ToList();
                stdResult.Course = new List<Cours>();
                for (int i = 0; i < stdResult.results.Count; i++)
                {
                    var courseId = stdResult.results[i].CourseId; // Retrieve the ProgrammeId from Std.Course[i] outside of the LINQ query

                    Cours Course = (from c in db.Results
                                    join r in db.Courses on c.CourseId equals r.CourseId
                                    where r.CourseId == courseId // Use the retrieved ProgrammeId in the query
                                    select r).FirstOrDefault();


                    stdResult.Course.Add(Course);
                }
                return View(stdResult);
            }
            return RedirectToAction("Index", "Student");
        }
        public ActionResult PrintAll()
        {
                if (Session["Student"] != null)
                {
                    var studentId = (Session["Student"] as Student).StudentId;
                    return new ActionAsPdf("PrintTranscript", new { studentId });
                }
                return RedirectToAction("Index", "Student");
            
        }

        public ActionResult Result(int ID)
        {
            if (Session["Student"] != null)
            {
                StdResult stdResult = new StdResult();
                stdResult.student = Session["Student"] as Student;
                stdResult.results = db.Results.Where(x => x.SemesterId == ID && x.StudentId == stdResult.student.StudentId).ToList();
                stdResult.Course = new List<Cours>();
                for (int i = 0; i < stdResult.results.Count; i++)
                {
                    var courseId = stdResult.results[i].CourseId; // Retrieve the ProgrammeId from Std.Course[i] outside of the LINQ query

                    Cours Course = (from c in db.Results
                                     join r in db.Courses on c.CourseId equals r.CourseId
                                         where r.CourseId == courseId // Use the retrieved ProgrammeId in the query
                                         select r).FirstOrDefault();


                    stdResult.Course.Add(Course);
                }
                ViewBag.Semester = ID;
                return View(stdResult);
            }
            return RedirectToAction("Index", "Student");
        }
        public ActionResult PrintSemResut(int ID, int studentId)
        {
            if (studentId!=0)
            {
                StdResult stdResult = new StdResult();
                stdResult.student = db.Students.Where(x => x.StudentId == studentId).FirstOrDefault();
                stdResult.results = db.Results.Where(x => x.SemesterId == ID && x.StudentId == stdResult.student.StudentId).ToList();
                stdResult.Course = new List<Cours>();
                for (int i = 0; i < stdResult.results.Count; i++)
                {
                    var courseId = stdResult.results[i].CourseId; // Retrieve the ProgrammeId from Std.Course[i] outside of the LINQ query

                    Cours Course = (from c in db.Results
                                     join r in db.Courses on c.CourseId equals r.CourseId
                                         where r.CourseId == courseId // Use the retrieved ProgrammeId in the query
                                         select r).FirstOrDefault();


                    stdResult.Course.Add(Course);
                }
                ViewBag.Semester = ID;
                return View(stdResult);
            }
            return RedirectToAction("Index", "Student");
        }
        public ActionResult SemResult(int ID)
        {
            if (Session["Student"] != null)
            {
                var studentId = (Session["Student"] as Student).StudentId;
                return new ActionAsPdf("PrintSemResut", new { ID , studentId });
            }
            return RedirectToAction("Index", "Student");

        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            try
            {
                User u = db.Users.Where((x) => x.Username == user.Username).FirstOrDefault();
                if (u != null)
                {
                    string salt = u.salt;
                    string hashedPassword = HashPassword(user.Password, salt);
                    if (hashedPassword == u.Password)
                    {
                        dynamic data;
                        if (u.RoleId == 1)
                        {
                            data = db.AdminStaffs.Where((X) => X.UserId == u.UserId).FirstOrDefault();
                            Session["Admin"] = data;
                            return RedirectToAction("AdminDashboard", "Admin");
                        }
                        else if (u.RoleId == 2)
                        {
                            data = db.Students.Where((X) => X.UserId == u.UserId).FirstOrDefault();
                            Session["Student"] = data;
                            return RedirectToAction("Dashboard", "Student");
                        }
                        else if (u.RoleId == 3)
                        {
                            data = db.Coordinators.Where((X) => X.UserId == u.UserId).FirstOrDefault();
                            Session["Coordinators"] = data;
                            return RedirectToAction("CoordinatorsDashboard", "Coordinators");
                        }
                        else if (u.RoleId == 4)
                        {
                            data = db.ExamBranches.Where((X) => X.UserId == u.UserId).FirstOrDefault();
                            Session["ExamBranch"] = data;
                            return RedirectToAction("Dashboard", "ExamBranch");
                        }
                        return RedirectToAction("Dashboard", "Student");
                    }
                    else
                    {
                        ViewBag.Error = "User Not Found Enter Correct Username Or Password";
                        TempData["User"] = ViewBag.Error;
                        return RedirectToAction("index", "Student");
                    }
                }
                else
                {
                    ViewBag.Error = "User Not Found Enter Correct Username Or Password";
                    TempData["User"] = ViewBag.Error;
                    return RedirectToAction("index", "Student");
                }
            }
            
            catch (Exception exp)
            {
                return RedirectToAction("index", "Student");
            }
            return View();
        }
        protected string HashPassword(string password, string salt)
        {
            // Combine the password and salt
            string combinedPassword = password + salt;

            // Choose the hash algorithm (SHA-256 or SHA-512)
            using (var sha256 = SHA256.Create())
            {
                // Convert the combined password string to a byte array
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(combinedPassword);

                // Compute the hash value of the byte array
                byte[] hash = sha256.ComputeHash(bytes);

                // Convert the byte array to a hexadecimal string
                System.Text.StringBuilder result = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    result.Append(hash[i].ToString("x2"));
                }

                return result.ToString();
            }
        }
        public ActionResult Logout()
        {
            // Clear the user's session data
            Session.Clear();

            // Redirect the user to the login page
            return RedirectToAction("Index", "Student");
        }
    }
}