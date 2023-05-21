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
        string databasePath = Path.Combine(FileSystem.CurrentDirectory, "MyDataR.db");
        SyncController syncController = new SyncController();


        [HttpGet]
        public async Task<IEnumerable<Parcela>> Get(int campingid)
        {
            var db = new SQLiteAsyncConnection(databasePath);
            var query = await db.Table<Parcela>().Where(x => x.campingid == campingid).ToListAsync();
            await db.CloseAsync();
            return query;
        }
        [HttpPut]
        public async Task Put(int campid, string parcela, int estadoid)
        {
            var db = new SQLiteAsyncConnection(databasePath);
            Parcela pe = await db.Table<Parcela>().FirstOrDefaultAsync(x => x.campingid == campid && x.numeroparcela == parcela);
            if (pe != null)
            {
                pe.estadoid = estadoid;
                await db.UpdateAsync(pe);
                await db.CloseAsync();
                await syncController.LoadData();
            }
            
        }
    }
}
