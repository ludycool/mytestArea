using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
 
namespace WcfService
{
    /// <summary>
    /// 模块实体
    /// </summary>
    [DataContract]
  public  class Module
    {
        [DataMember]
        public string ModuleNO;
        [DataMember]
        public string ModuleName;
    }
}