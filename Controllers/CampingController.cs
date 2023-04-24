using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PFGWS.Models;
using SQLite;
using Microsoft.VisualBasic.FileIO;

namespace PFGWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampingController : Controller
    {
        string databasePath = Path.Combine(FileSystem.CurrentDirectory, "MyData.db");



        [HttpGet]
        public async Task<IEnumerable<Camping>> Get()
        {
            var db = new SQLiteAsyncConnection(databasePath);
            var query = await db.Table<Camping>().ToListAsync();
            await db.CloseAsync();

            return query;
        }
    }
}
