using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFServiceDaemon
{
    [ServiceContract]
    public interface IStudentService
    {
        [OperationContract]
        string GetStudentFullName(int studentId);

        [OperationContract]
        IEnumerable<StudentInfo> GetStudentInfo(int studentId);
    }

    [DataContract]
    public class StudentInfo
    {
        int studentID;
        string lastName;
        string firstName;

        [DataMember]
        public int StudentID
        {
            get { return studentID; }
            set { studentID = value; }
        }

        [DataMember]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        [DataMember]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
    }
}
