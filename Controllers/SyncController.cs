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
        static SQLiteAsyncConnection dbR;


        static async Task Init()
        {   
            try
            {
                // Get an absolute path to the database file
                //var databasePath = Path.Combine(@"sqlite\MyDataA.db");
                var databasePathR = Path.Combine(FileSystem.CurrentDirectory, "MyDataR.db");
                dbR = new SQLiteAsyncConnection(databasePathR);
                await dbR.CreateTableAsync<Reserva>();
                await dbR.CreateTableAsync<Camping>();
                await dbR.CreateTableAsync<Estado>();
                await dbR.CreateTableAsync<Producto>();
                await dbR.CreateTableAsync<Parcela>();
                await dbR.CreateTableAsync<User>();
                await dbR.CreateTableAsync<Suscripcion>();
                await dbR.CloseAsync();

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
        [Route("~/api/Sync/Deprovision")]
        public async Task<string> Deprovision()
        {
            try
            {
                SqlSyncProvider serverProvider = new SqlSyncProvider(@"Server=tcp:pfg.database.windows.net,1433;Initial Catalog=PFG;User ID=almata;Password=vH3Q7v29H!v");
                var databasePath = Path.Combine(FileSystem.CurrentDirectory, "MyDataR.db");
                SqliteSyncProvider clientProvider = new SqliteSyncProvider(databasePath);

                var remoteOrchestrator = new RemoteOrchestrator(serverProvider);
                
                // Deprovision everything
                var p = SyncProvision.StoredProcedures | SyncProvision.TrackingTable |
                        SyncProvision.Triggers;

                // Deprovision everything
                var localOrchestrator = new LocalOrchestrator(clientProvider);
                await remoteOrchestrator.DropAllAsync();
                await localOrchestrator.DropAllAsync();
                return "Ok";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }

        }
        [HttpGet]
        [Route("~/api/Sync/Load")]
        public async Task<string> LoadData()
        {
            try
            {
                SqlSyncProvider serverProvider = new SqlSyncProvider(@"Server=tcp:pfg.database.windows.net,1433;Initial Catalog=PFG;User ID=almata;Password=vH3Q7v29H!v");
                var databasePath = Path.Combine(FileSystem.CurrentDirectory, "MyDataR.db");

                SqliteSyncProvider clientProvider = new SqliteSyncProvider(databasePath);
                
                var tablas = new string[] { "Reserva", "Camping", "Estado","Producto", "Parcela","Suscripcion", "Users"};
                
                var setup = new SyncSetup(tablas);
                
                var agent = new SyncAgent(clientProvider, serverProvider);
                var s1 = await agent.SynchronizeAsync(setup);
                var result = await agent.SynchronizeAsync();
                return "Ok";
                //agent.Dispose();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }

        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                await Init();
                await LoadData();
                var path = Path.Combine(FileSystem.CurrentDirectory, "MyDataR.db");
                var path2 = Path.Combine(FileSystem.CurrentDirectory, "MyDataRCopy.db");

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
