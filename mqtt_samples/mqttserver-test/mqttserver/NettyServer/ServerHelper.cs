using System;
using System.Collections.Generic;
using System.Text;

namespace NettyServer
{
  public  class ServerHelper
    {

        public static string ProcessDirectory
        {
            get
            {
#if NETSTANDARD1_3
                return AppContext.BaseDirectory;
#else
                return AppDomain.CurrentDomain.BaseDirectory;
#endif
            }
        }
    }
}
