using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Data.Linq;
using System.Diagnostics;
using System.Security.Principal;

namespace MVCWithLinq1.Models
{
    public class StudentDAL
    {
        MVCDBDataContext dc = new MVCDBDataContext(ConfigurationManager.ConnectionStrings["MVCDBConnectionString"].ConnectionString);

        public List<Student> GetStudents(bool? Status)
        {
            List<Student> students = new List<Student>();
            try
            {
                if (Status != null)
                    students = (from S in dc.Students where S.Status == Status select S).ToList();
                else
                    students = (from S in dc.Students select S).ToList();
            }
            catch (Exception ex)
            {
                Trace.TraceError("StudentDAL.GetStudents: " + ex.Message);
                throw ex;
            }
            return students; 
        }
        public Student GetStudent(int Sid, bool? Status)
        {
            Student student = null;
            try
            {
                if (Status != null)
                    student = (from S in dc.Students where S.Sid == Sid && S.Status == Status select S).Single();
                else
                    student = (from S in dc.Students where S.Sid == Sid select S).Single();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return student;
        }
        public void InsertStudent(Student student)
        {
            try
            {
                dc.Students.InsertOnSubmit(student);
                dc.SubmitChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateStudent(Student newValues)
        {
            try
            {
                Student oldValues = dc.Students.First(S => S.Sid == newValues.Sid);
                oldValues.Name = newValues.Name;
                oldValues.Fees = newValues.Fees;
                oldValues.Class = newValues.Class;
                oldValues.Photo = newValues.Photo;
                dc.SubmitChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteStudent(int Sid)
        {
            try
            {
                Student student = dc.Students.First(S => S.Sid == Sid);
                //Code for permanent delete
                //dc.Students.DeleteOnSubmit(student);

                //Code for updating status without permanent delete
                student.Status = false;
                dc.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}