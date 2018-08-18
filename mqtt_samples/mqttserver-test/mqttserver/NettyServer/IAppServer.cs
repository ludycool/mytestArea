using System.Threading.Tasks;

namespace NettyServer
{
    public interface IAppServer
    {
        Task CloseServer();
        Task<bool> startServer(ServerConfig _config);



    }

}
