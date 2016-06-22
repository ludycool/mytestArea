using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace WCFServiceHostInConsole
{
    [ServiceContract]
    public interface IStudentService
    {
        [OperationContract]
        string GetStudentFullName(int studentId);

        [OperationContract]
        IEnumerable<StudentInfo> GetStudentInfo(int studentId);
    }
}