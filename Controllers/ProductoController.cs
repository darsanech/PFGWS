using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PFGWS.Models;
using SQLite;
using Microsoft.VisualBasic.FileIO;
using Microsoft.AspNetCore.Authorization;
using System.Collections.ObjectModel;

namespace PFGWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ProductoController : Controller
    {
        string databasePath = Path.Combine(FileSystem.CurrentDirectory, "MyDataFFF.db");

        SyncController syncController = new SyncController();

        [HttpGet]
        public async Task<IEnumerable<Producto>> Get()
        {
            var db = new SQLiteAsyncConnection(databasePath);
            var query = await db.Table<Producto>().ToListAsync();
            await db.CloseAsync();

            return query;
        }
        [HttpPut]
        public async Task BasicPut(int producteid, int mod, bool total, bool sale) //false disponible-true total  //sale=true resta, sale=False es recogido
        {
            var db = new SQLiteAsyncConnection(databasePath);
            Producto prod = await db.Table<Producto>().FirstOrDefaultAsync(x => x.producteid == producteid);
            if (total)
            {
                prod.total += mod;
                prod.disponible += mod;
                await db.UpdateAsync(prod);
                await syncController.LoadData();
            }
            else
            {
                if (sale)
                {
                    prod.disponible -= mod;
                }
                else
                {
                    prod.disponible += mod;
                }
                await db.UpdateAsync(prod);

            }
            await db.CloseAsync();

        }
    }
}
