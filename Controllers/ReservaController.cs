using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PFGWS.Models;
using SQLite;
using Microsoft.VisualBasic.FileIO;

namespace PFGWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : Controller
    {
        string databasePath = Path.Combine(FileSystem.CurrentDirectory, "MyData.db");

        [HttpPost]
        public async void Post([FromBody] Reserva reserva)
        {
            var db = new SQLiteAsyncConnection(databasePath);
            await db.InsertAsync(reserva);
            await db.CloseAsync();
        }


        [HttpGet]
        public async Task<IEnumerable<Reserva>> Get()
        {
            var db = new SQLiteAsyncConnection(databasePath);
            var query = await db.Table<Reserva>().ToListAsync();
            await db.CloseAsync();

            return query;
        }
        [HttpPut]
        public async void Put([FromBody] Reserva reserva)
        {
            var db = new SQLiteAsyncConnection(databasePath);
            await db.UpdateAsync(reserva);
            await db.CloseAsync();
        }
    }
}
