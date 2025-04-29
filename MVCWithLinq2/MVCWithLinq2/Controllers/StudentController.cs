using MVCWithLinq2.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Linq;
using System.IO;

namespace MVCWithLinq2.Controllers
{
    public class StudentController : Controller
    {
        MVCDBDataContext dc = new MVCDBDataContext(ConfigurationManager.ConnectionStrings["MVCDBConnectionString"].ConnectionString);

        public ViewResult DisplayStudents()
        {
            var students = dc.Student_Select(null, true);
            return View(students);
        }
        public ViewResult DisplayStudent(int Sid)
        {
            var student = dc.Student_Select(Sid, true).Single();
            return View(student);
        }
        [HttpGet]
        public ViewResult AddStudent()
        { 
            return View(); 
        }
        [HttpPost]
        public RedirectToRouteResult AddStudent(Student_SelectResult student, HttpPostedFileBase selectedFile)
        {
            if(selectedFile != null)
            {
                string folderPath = Server.MapPath("~/Uploads/");
                if(!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                selectedFile.SaveAs(folderPath + selectedFile.FileName);
                student.Photo = selectedFile.FileName;
            }
            dc.Student_Insert(student.Sid, student.Name, student.Class, student.Fees, student.Photo);
            return RedirectToAction("DisplyStudents");
        }
        public ViewResult EditStudent(int Sid)
        {
            var student = dc.Student_Select(Sid, true).Single();
            TempData["Photo"] = student.Photo;
            return View(student);
        }
        public RedirectToRouteResult UpdateStudent(Student_SelectResult student, HttpPostedFileBase selectedFile)
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
            dc.Student_Update(student.Sid, student.Name, student.Class, student.Fees,student.Photo);
            return RedirectToAction("DisplayStudents");
        }
        public RedirectToRouteResult DeleteStudent(int Sid)
        {
            dc.Student_Delete(Sid);
            return RedirectToAction("DisplayStudents");
        }
    }
}