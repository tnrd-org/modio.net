using System.Threading.Tasks;

using Modio.Filters;
using Modio.Models;

namespace Modio
{
    public class ModClient : ApiClient
    {
        public uint GameId { get; private set; }

        public uint ModId { get; private set; }

        public FilesClient Files { get; private set; }

        internal ModClient(IConnection connection, uint game, uint mod) : base(connection)
        {
            GameId = game;
            ModId = mod;
            Files = new FilesClient(connection, game, mod);
        }

        public async Task<Mod> Get()
        {
            var (method, path) = Routes.GetMod(GameId, ModId);
            var req = new Request(method, Connection.BaseAddress, path);

            return await Connection.Send<Mod>(req);
        }

        public SearchClient<ModEvent> GetEvents(Filter? filter = null)
        {
            var route = Routes.GetModEvents(GameId, ModId);
            return new SearchClient<ModEvent>(Connection, route, filter);
        }

        public async Task<Statistics> GetStatistics()
        {
            var (method, path) = Routes.GetModStats(GameId, ModId);
            var req = new Request(method, Connection.BaseAddress, path);

            return await Connection.Send<Statistics>(req);
        }
    }
}
