using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using Dotmim.Sync;
using Dotmim.Sync.Sqlite;
using Dotmim.Sync.SqlServer;
using SQLite;
using PFGWS.Models;

namespace PFGWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OtroController : ControllerBase
    {
        private readonly ILogger<OtroController> _logger;
        static SQLiteAsyncConnection db;


        static async Task Init()
        {
            try
            {
                // Get an absolute path to the database file
                var databasePath = Path.Combine(@"sqlite\MyData.db");

                db = new SQLiteAsyncConnection(databasePath);

                await db.CreateTableAsync<Reserva>();


                await db.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public OtroController(ILogger<OtroController> logger)
        {
            
            _logger = logger;
        }
        static async Task LoadData()
        {
            try
            {
                SqlSyncProvider serverProvider = new SqlSyncProvider(@"Server=tcp:pfg.database.windows.net,1433;Initial Catalog=PFG;User ID=almata;Password=vH3Q7v29H!v");
                SqliteSyncProvider clientProvider = new SqliteSyncProvider("sqlite/MyData.db");
                var setup = new SyncSetup("Reserva");

                var agent = new SyncAgent(clientProvider, serverProvider);
                var s1 = await agent.SynchronizeAsync(setup);
                var result = await agent.SynchronizeAsync();
                agent.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                await Init();
                await LoadData();

                var path = Path.Combine(@"sqlite\MyData.db");
                var path2 = Path.Combine(@"sqlite\MyData2.db");
                if (System.IO.File.Exists(path2))
                {
                    System.IO.File.Delete(path2);
                }
                System.IO.File.Copy(path, path2);
                var stream = System.IO.File.OpenRead(path2);

                return new FileStreamResult(stream, "application/octet-stream");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
