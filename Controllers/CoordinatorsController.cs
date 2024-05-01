using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FYPWeb.Model;
using FYPWeb.DataModel;
using System.Data.Entity.Validation;
using System.IO;
using System.Threading.Tasks;
using ExcelDataReader;

namespace FYPWeb.Controllers
{
    public class CoordinatorsController : Controller
    {
        NUMLAutomatedTranscriptEntities db = new NUMLAutomatedTranscriptEntities();
        // GET: Coordinators
        public ActionResult CoordinatorsDashboard()
        {
            if (Session["Coordinators"] != null)
            {
                ManageCourse Std = new ManageCourse();
                Std.coordinator = Session["Coordinators"] as Coordinator;
                return View(Std);
            }
            return RedirectToAction("Index", "Student");
        }
        public ActionResult CoordinatorsDashboardtest()
        {
            if (Session["Coordinators"] != null)
            {
                ManageCourse Std = new ManageCourse();
                Std.coordinator = Session["Coordinators"] as Coordinator;
                return View(Std);
            }
            return RedirectToAction("Index", "Student");
        }
        public ActionResult AssignCourse()
        {
            if (Session["Coordinators"] != null)
            {
                ManageCourse Std = new ManageCourse();
                Std.coordinator = Session["Coordinators"] as Coordinator;

                Std.Courses = db.Courses.ToList();
                return View(Std);
            }
            return RedirectToAction("Index", "Student");
        }

        [HttpPost]
        public ActionResult AssignNewCourse(ManageCourse studentUser)
        {
            try
            {
                // Check if the student already exists
                var existingCourse = db.StudentsCourses.FirstOrDefault(x => x.CourseId == studentUser.studentsCours.CourseId && x.SemesterNo == studentUser.studentsCours.SemesterNo);


                if (existingCourse == null)
                {
                    int maxStudentId = db.StudentsCourses.Max(s => (int?)s.CourseId) ?? 0;

                    // Increment the maximum StudentId by 1
                    int newStudentId = maxStudentId + 1;
                    studentUser.studentsCours.StudentCourseId = newStudentId;
                    // Add the student and user to the database
                    db.StudentsCourses.Add(studentUser.studentsCours);


                    // Save changes to the database
                    db.SaveChanges();

                    // Redirect to CreateProfile action upon successful save
                    return RedirectToAction("CoordinatorsDashboard", "Coordinators");
                }
                else
                {
                    // Handle case where student already exists
                    ModelState.AddModelError("", "A student with the same name already exists.");
                    return RedirectToAction("AssignCourse", "Coordinators");
                }
            }
            catch (DbEntityValidationException ex)
            {
                // Handle validation errors
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        ModelState.AddModelError("", validationError.ErrorMessage);
                    }
                }
                return RedirectToAction("AssignCourse", "Coordinators");
            }
            catch (Exception exp)
            {
                // Handle other exceptions
                ModelState.AddModelError("", "An error occurred while creating the student. Please try again later.");
                Console.WriteLine(exp.Message);
                if (exp.InnerException != null)
                {
                    // Print the inner exception's message
                    Console.WriteLine("Inner Exception: " + exp.InnerException.Message);
                }
                return RedirectToAction("AssignCourse", "Coordinators");
            }
        }
        public ActionResult AssignRepeatCourse()
        {
            if (Session["Coordinators"] != null)
            {
                ManageCourse Std = new ManageCourse();
                Std.coordinator = Session["Coordinators"] as Coordinator;

                Std.Courses = db.Courses.ToList();
                Std.programmes = db.Programmes.ToList();
                return View(Std);
            }
            return RedirectToAction("Index", "Student");
        }

        [HttpPost]
        public ActionResult AssignNewRepeatCourse(ManageCourse studentUser)
        {
            try
            {
                // Check if the student already exists
                var existingCourse = db.RepeatedCourses.FirstOrDefault(x => x.CourseId == studentUser.RepeatedCours.CourseId && x.StudentId == studentUser.RepeatedCours.StudentId);
                var existingStd = db.Students.FirstOrDefault(x => x.StudentId == studentUser.RepeatedCours.StudentId);

                if (existingCourse == null && existingStd != null)
                {
                    int maxStudentId = db.RepeatedCourses.Max(s => (int?)s.CourseId) ?? 0;

                    // Increment the maximum StudentId by 1
                    int newStudentId = maxStudentId + 1;
                    studentUser.RepeatedCours.RepeatedCourseId = newStudentId;
                    studentUser.RepeatedCours.ObtainMarks = 0;
                    // Add the student and user to the database
                    db.RepeatedCourses.Add(studentUser.RepeatedCours);


                    // Save changes to the database
                    db.SaveChanges();

                    // Redirect to CreateProfile action upon successful save
                    return RedirectToAction("CoordinatorsDashboard", "Coordinators");
                }
                else
                {
                    // Handle case where student already exists
                    ModelState.AddModelError("", "A student with the same name already exists.");
                    return RedirectToAction("AssignCourse", "Coordinators");
                }
            }
            catch (DbEntityValidationException ex)
            {
                // Handle validation errors
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        ModelState.AddModelError("", validationError.ErrorMessage);
                    }
                }
                return RedirectToAction("AssignCourse", "Coordinators");
            }
            catch (Exception exp)
            {
                // Handle other exceptions
                ModelState.AddModelError("", "An error occurred while creating the student. Please try again later.");
                Console.WriteLine(exp.Message);
                if (exp.InnerException != null)
                {
                    // Print the inner exception's message
                    Console.WriteLine("Inner Exception: " + exp.InnerException.Message);
                }
                return RedirectToAction("AssignCourse", "Coordinators");
            }
        }

        public ActionResult AddMarks()
        {
            if (Session["Coordinators"] != null)
            {
                ManageCourse Std = new ManageCourse();
                Std.coordinator = Session["Coordinators"] as Coordinator;
                Std.Courses = db.Courses.ToList();
                Std.students = db.Students.ToList();
                string message = TempData["Marks"] as string; // Retrieve message from TempData
                if (!string.IsNullOrEmpty(message))
                {
                    ViewBag.Marks = message;
                }
                return View(Std);
            }
            return RedirectToAction("Index", "Student");
        }

        [HttpPost]
        public ActionResult AddNewMarks(ManageCourse studentUser)
        {
            try
            {
                // Check if the student already exists
                var existingStd = db.Students.FirstOrDefault(x => x.StudentId == studentUser.result.StudentId);

                if (existingStd != null)
                {
                    Result resultExit = db.Results.Where((x) => x.SemesterId == studentUser.result.SemesterId && x.CourseId == studentUser.result.CourseId).FirstOrDefault();
                    if (resultExit == null)
                    {
                        int maxResultId = db.Results.Max(s => (int?)s.CourseId) ?? 0;
                        int newResultId = maxResultId + 1;
                        studentUser.result.ResultId = newResultId;

                        db.Results.Add(studentUser.result);
                        db.SaveChanges();
                    }
                    else
                    {
                        resultExit = studentUser.result;
                        db.SaveChanges();
                    }

                    var allResult = db.Results.Where((x) => x.StudentId == existingStd.StudentId).ToList();
                    // Add the student and user to the database
                    decimal? TotalCGPA = 0;
                    if (allResult != null)
                    {

                        for (int i = 0; i < 11; i++)
                        {
                            decimal? GPA = 0;
                            int semesterNo = i + 1;
                            var semesterResult = allResult.Where((x) => x.StudentId == semesterNo).ToList();
                            if (semesterResult.Count == 0)
                            {
                                break;
                            }
                            else
                            {
                                decimal? totalObtainCredit = 0;
                                for (int j = 0; j < semesterResult.Count; j++)
                                {
                                    var currentCouseId = semesterResult[i].CourseId;
                                    Cours stdCourse = db.Courses.Where((x) => x.CourseId == currentCouseId).FirstOrDefault();
                                    totalObtainCredit += (semesterResult[j].ObtainMarks / stdCourse.TotalMarks) * stdCourse.TotalCreditHours;
                                }
                                GPA += totalObtainCredit / 4;
                            }
                            if (TotalCGPA != 0)
                            {
                                TotalCGPA = (TotalCGPA + GPA) / 2;
                            }
                            else
                            {
                                TotalCGPA = GPA;
                            }
                        }
                    }
                    existingStd.CGPA = (decimal)TotalCGPA;

                    // Save changes to the database
                    db.SaveChanges();

                    // Redirect to CreateProfile action upon successful save
                    return RedirectToAction("CoordinatorsDashboard", "Coordinators");
                }
                else
                {
                    // Handle case where student already exists
                    ModelState.AddModelError("", "A student with the same name already exists.");
                    return RedirectToAction("AssignCourse", "Coordinators");
                }
            }
            catch (DbEntityValidationException ex)
            {
                // Handle validation errors
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        ModelState.AddModelError("", validationError.ErrorMessage);
                    }
                }
                return RedirectToAction("AssignCourse", "Coordinators");
            }
            catch (Exception exp)
            {
                // Handle other exceptions
                ModelState.AddModelError("", "An error occurred while creating the student. Please try again later.");
                Console.WriteLine(exp.Message);
                if (exp.InnerException != null)
                {
                    // Print the inner exception's message
                    Console.WriteLine("Inner Exception: " + exp.InnerException.Message);
                }
                return RedirectToAction("AssignCourse", "Coordinators");
            }
        }
        public ActionResult GetCoursesBySemester(int semesterId)
        {
            var courses = (from c in db.Courses
                           join sc in db.StudentsCourses on c.CourseId equals sc.CourseId
                           where sc.SemesterNo == semesterId
                           select c).ToList();
            return Json(courses, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStudentsBySemesterAndCourse(int semesterId, int courseId)
        {


            var students = (from c in db.Students
                            join sc in db.StudentsCourses on c.CurrentSemester equals sc.SemesterNo
                            where sc.CourseId == courseId
                            select new
                            {
                                StudentId = c.StudentId,
                                Name = c.Name
                            }).ToList();
            return Json(students, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
public async Task<ActionResult> Uploadexcelfile(HttpPostedFileBase file)
{
    if (file != null && file.ContentLength > 0)
    {
        var fileName = Path.GetFileName(file.FileName);
        var uploadsFolder = Path.Combine(Server.MapPath("~/Upload"), fileName);

        using (var stream = new FileStream(uploadsFolder, FileMode.Create))
        {
            await file.InputStream.CopyToAsync(stream);
        }

        using (var stream = System.IO.File.Open(uploadsFolder, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                // Assuming the first row is a header, so skip it
                reader.Read();

                while (reader.Read()) // Read each row in the Excel sheet
                {
                    Result Rs = new Result();
                    ManageCourse mc = new ManageCourse();
                    int maxResultId = db.Results.Max(s => (int?)s.StudentId) ?? 0;
                    int newResultId = maxResultId + 1;
                    Rs.ResultId = newResultId;
                    Rs.ObtainMarks = Convert.ToInt32(reader.GetValue(1));
                    int rollNumber = Convert.ToInt32(reader.GetValue(2));
                    // Retrieve the student from the database using LINQ
                    mc.student = db.Students.FirstOrDefault(x => x.RollNumber == rollNumber);
                    Rs.StudentId = mc.student.StudentId;
                    string courseCode = reader.GetValue(3).ToString();
                    mc.Course = db.Courses.FirstOrDefault(x => x.CourseCode == courseCode);
                    Rs.CourseId = mc.Course.CourseId;
                    Rs.SemesterId = Convert.ToInt32(reader.GetValue(4));
                    db.Results.Add(Rs);
                        }

                        // Save changes to the database after reading all rows from the Excel file
                        //db.SaveChanges();
                        ViewBag.Successfully = "Add All Marks Successfully";
                        TempData["Marks"] = ViewBag.Successfully;
                        return RedirectToAction("AddMarks");
                    }
                }
    }
            else
            {
                ViewBag.Message = "empty";
            }

    // Optionally, you can perform additional processing with the uploaded file here
    return RedirectToAction("Index");
}


    }
}