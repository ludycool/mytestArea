using System.Collections.Generic;
using System.ServiceModel;

namespace WCFServiceGeneratedByConfig
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