using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using Dotmim.Sync;
using Dotmim.Sync.Sqlite;
using Dotmim.Sync.SqlServer;
using SQLite;
using PFGWS.Models;
using System.IO;
using System;
using Microsoft.VisualBasic.FileIO;
using Dotmim.Sync.Enumerations;
using Microsoft.AspNetCore.Authorization;

namespace PFGWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class SyncController : ControllerBase
    {
        private readonly ILogger<SyncController> _logger;
        static SQLiteAsyncConnection db;


        static async Task Init()
        {
            try
            {
                // Get an absolute path to the database file
                //var databasePath = Path.Combine(@"sqlite\MyData.db");
                var databasePath = Path.Combine(FileSystem.CurrentDirectory, "MyData.db");
                System.IO.File.Delete(databasePath);



                db = new SQLiteAsyncConnection(databasePath);
                await db.CreateTableAsync<Reserva>();
                await db.CreateTableAsync<Camping>();
                await db.CreateTableAsync<Estado>();
                await db.CreateTableAsync<Producto>();
                await db.CreateTableAsync<User>();
                await db.CreateTableAsync<Parcela>();
                await db.CreateTableAsync<Suscripcion>();



                await db.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public SyncController()
        {
            
        }

        [HttpGet]
        [Route("~/api/Sync/Load")]
        public async Task LoadData()
        {
            try
            {
                SqlSyncProvider serverProvider = new SqlSyncProvider(@"Server=tcp:pfg.database.windows.net,1433;Initial Catalog=PFG;User ID=almata;Password=vH3Q7v29H!v");
                var databasePath = Path.Combine(FileSystem.CurrentDirectory, "MyData.db");
                SqliteSyncProvider clientProvider = new SqliteSyncProvider(databasePath);
                
                var tablas = new string[] { "Reserva", "Camping", "Estado","Producto", "Users" , "Parcela","Suscripcion"};
                /*
                var remoteOrchestrator = new RemoteOrchestrator(serverProvider);
                
                // Deprovision everything
                var p = SyncProvision.StoredProcedures | SyncProvision.TrackingTable |
                        SyncProvision.Triggers;

                // Deprovision everything
                var localOrchestrator = new LocalOrchestrator(clientProvider);
                await remoteOrchestrator.DropAllAsync();
                await localOrchestrator.DropAllAsync();
                
                */
                var setup = new SyncSetup(tablas);
                
                var agent = new SyncAgent(clientProvider, serverProvider);
                var s1 = await agent.SynchronizeAsync(setup);
                var result = await agent.SynchronizeAsync();
                
                //agent.Dispose();
                
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
                var path = Path.Combine(FileSystem.CurrentDirectory, "MyData.db");
                var path2 = Path.Combine(FileSystem.CurrentDirectory, "MyData2.db");

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
