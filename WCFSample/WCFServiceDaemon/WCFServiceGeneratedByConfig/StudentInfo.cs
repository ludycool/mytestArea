using System.Runtime.Serialization;

namespace WCFServiceGeneratedByConfig
{
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
