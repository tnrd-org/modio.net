using System.Threading.Tasks;

using Modio.Filters;
using Modio.Models;

namespace Modio
{
    public class ModClient : ApiClient
    {
        public uint GameId { get; private set; }

        public uint ModId { get; private set; }

        public TagsClient Tags { get; private set; }

        public DependenciesClient Dependencies { get; private set; }

        public FilesClient Files { get; private set; }

        public CommentsClient Comments { get; private set; }

        public TeamsClient Team { get; private set; }

        internal ModClient(IConnection connection, uint game, uint mod) : base(connection)
        {
            GameId = game;
            ModId = mod;
            Tags = new TagsClient(connection, game, mod);
            Dependencies = new DependenciesClient(connection, game, mod);
            Files = new FilesClient(connection, game, mod);
            Comments = new CommentsClient(connection, game, mod);
            Team = new TeamsClient(connection, game, mod);
        }

        public async Task<Mod> Get()
        {
            var (method, path) = Routes.GetMod(GameId, ModId);
            var req = new Request(method, Connection.BaseAddress, path);
            var resp = await Connection.Send<Mod>(req);
            return resp.Body!;
        }

        public async Task<Mod> Edit(EditMod editMod)
        {
            using (var content = editMod.ToContent())
            {
                var (method, path) = Routes.EditMod(GameId, ModId);
                var req = new Request(method, Connection.BaseAddress, path);
                req.Body = content;

                var resp = await Connection.Send<Mod>(req);
                return resp.Body!;
            }
        }

        public async Task Delete()
        {
            var (method, path) = Routes.DeleteMod(GameId, ModId);
            var req = new Request(method, Connection.BaseAddress, path);
            await Connection.Send<ApiMessage>(req);
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
            var resp = await Connection.Send<Statistics>(req);
            return resp.Body!;
        }
    }
}
