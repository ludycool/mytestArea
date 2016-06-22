using System.Collections.Generic;
using System.Linq;

namespace WCFServiceGeneratedByConfig
{
    public class StudentService : IStudentService
    {

        List<StudentInfo> list = new List<StudentInfo>();

        public StudentService()
        {
            list.Add(new StudentInfo { StudentID = 10010, FirstName = "Shi", LastName = "Chaoyang" });
            list.Add(new StudentInfo { StudentID = 10011, FirstName = "Liu", LastName = "Jieus" });
            list.Add(new StudentInfo { StudentID = 10012, FirstName = "Cheung", LastName = "Vincent" });
            list.Add(new StudentInfo { StudentID = 10013, FirstName = "Yang", LastName = "KaiVen" });
        }

        public string GetStudentFullName(int studentId)
        {
            IEnumerable<string> Student = from p in list
                                          where p.StudentID == studentId
                                          select p.FirstName + " " + p.LastName;

            return Student.Count() != 0 ? Student.First() : string.Empty;
        }

        public IEnumerable<StudentInfo> GetStudentInfo(int studentId)
        {
            IEnumerable<StudentInfo> Student = from p in list
                                               where p.StudentID == studentId
                                               select p;
            return Student;
        }
    }
}
