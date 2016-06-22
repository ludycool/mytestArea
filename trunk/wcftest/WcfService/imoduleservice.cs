using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfService
{
    /// <summary>
    /// 接口类
    /// </summary>
    [ServiceContract]
    public interface imoduleservice
    {
        [OperationContract]
        void AddModules(Module book);

        [OperationContract]
        Module GetModuleByID(string id);

        [OperationContract]
        void RemoveModule(string id);

        [OperationContract]
        void ModuleUpdate(Module book);
    }
}
