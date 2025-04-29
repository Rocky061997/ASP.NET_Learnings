using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCWithLinq1.Models;

namespace MVCWithLinq1.Controllers
{
    public class StudentController : Controller
    {
       StudentDAL dal = new StudentDAL();
       public ViewResult DisplayStudents()
       {
            List<Student> students = dal.GetStudents(true);
            return View(students);
       }
       public ViewResult DisplayStudent(int Sid)
       {
            var student = dal.GetStudent(Sid, true);
            return View(student);
       }
       [HttpGet]
       public ViewResult AddStudent()
       {
            return View();
       }
       [HttpPost]
       public RedirectToRouteResult AddStudent(Student student, HttpPostedFileBase selectedFile)
       {
            if(selectedFile != null)
            {
                string folderPath = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                selectedFile.SaveAs(folderPath + selectedFile.FileName);
                student.Photo = selectedFile.FileName;
            }

            student.Status = true;
            dal.InsertStudent(student);
            return RedirectToAction("DisplayStudents");
       }
       public ViewResult EditStudent(int Sid)
       {
            Student student = dal.GetStudent(Sid, true);
            TempData["Photo"] = student.Photo;
            return View(student);
       }
       public RedirectToRouteResult UpdateStudent(Student student, HttpPostedFileBase selectedFile)
       {
            if (selectedFile != null)
            {
                string folderPath = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                selectedFile.SaveAs(folderPath + selectedFile.FileName);
                student.Photo = selectedFile.FileName;
            }
            else if (TempData["Photo"] != null)
                student.Photo = TempData["Photo"].ToString();
            dal.UpdateStudent(student);
            return RedirectToAction("DisplayStudents");
       }
       public RedirectToRouteResult DeleteStudent(int Sid)
       {
           dal.DeleteStudent(Sid);
           return RedirectToAction("DisplayStudents");
       }
    }
}