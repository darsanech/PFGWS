using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using PFGWS.Models;
using SQLite;
using System.Security.Claims;

namespace PFGWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParcelaController : Controller
    {
        string databasePath = Path.Combine(FileSystem.CurrentDirectory, "MyDataRest.db");
        SyncController syncController = new SyncController();


        [HttpGet]
        public async Task<IEnumerable<ParcelaEstado>> Get()
        {
            var db = new SQLiteAsyncConnection(databasePath);
            var query = await db.Table<ParcelaEstado>().ToListAsync();
            await db.CloseAsync();
            return query;
        }
        [HttpPut]
        public async Task Put(int campid, string parcela, int estadoid)
        {
            ParcelaEstado pe = new ParcelaEstado();
            pe.campingid= campid;
            pe.estadoid= estadoid;
            pe.numeroparcela = parcela;
            var db = new SQLiteAsyncConnection(databasePath);
            await db.UpdateAsync(pe);
            await db.CloseAsync();
            await syncController.LoadData();
        }
    }
}
