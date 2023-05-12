using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PFGWS.Models;
using SQLite;
using Microsoft.VisualBasic.FileIO;
using Microsoft.AspNetCore.Authorization;

namespace PFGWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ProductoController : Controller
    {
        string databasePath = Path.Combine(FileSystem.CurrentDirectory, "MyData.db");



        [HttpGet]
        public async Task<IEnumerable<Producto>> Get()
        {
            var db = new SQLiteAsyncConnection(databasePath);
            var query = await db.Table<Producto>().ToListAsync();
            await db.CloseAsync();

            return query;
        }
    }
}
