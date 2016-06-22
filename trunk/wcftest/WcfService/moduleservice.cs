using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace WcfService
{ /// <summary>
    /// 接口类实现
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
public    class moduleservice : imoduleservice
    {
        #region imoduleservice 成员
        List<Module> _Modules = new List<Module>();

        public void AddModules(Module m)
        {
            m.ModuleNO = Guid.NewGuid().ToString();
            _Modules.Add(m);
        }

        public Module GetModuleByID(string id)
        {
            Module m = _Modules.Find(p => p.ModuleNO == id);
            return m;
        }

        public void RemoveModule(string id)
        {
            Module m = _Modules.Find(p => p.ModuleNO == id);
            _Modules.Remove(m);
        }

        public void ModuleUpdate(Module module)
        {
            Module m = _Modules.Find(p => p.ModuleNO == module.ModuleNO);
            m.ModuleName = module.ModuleName;
        }

        #endregion
    }
}