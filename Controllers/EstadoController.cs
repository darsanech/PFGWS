using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PFGWS.Models;
using SQLite;
using Microsoft.VisualBasic.FileIO;

namespace PFGWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoController : Controller
    {
        string databasePath = Path.Combine(FileSystem.CurrentDirectory, "MyData.db");



        [HttpGet]
        public async Task<IEnumerable<EstadoProducto>> Get()
        {
            var db = new SQLiteAsyncConnection(databasePath);
            var query = await db.Table<EstadoProducto>().ToListAsync();
            await db.CloseAsync();
            return query;
        }
    }
}
