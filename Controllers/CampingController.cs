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

    public class CampingController : Controller
    {
        string databasePath = Path.Combine(FileSystem.CurrentDirectory, "MyDataFFF.db");



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
