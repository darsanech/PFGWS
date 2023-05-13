using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PFGWS.Models;
using SQLite;
using Microsoft.VisualBasic.FileIO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace PFGWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservaController : Controller
    {
        string databasePath = Path.Combine(FileSystem.CurrentDirectory, "MyData.db");

        [HttpPost]

        public async void Post([FromBody] Reserva reserva)
        {
            var db = new SQLiteAsyncConnection(databasePath);
            await db.InsertAsync(reserva);
            string Rol = User.FindFirst(ClaimTypes.Role).Value;
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
        [HttpGet]
        [Route("/api/Reserva/GetUser")]
        public async Task<IEnumerable<Reserva>> GetUser()
        {
            var db = new SQLiteAsyncConnection(databasePath);
            var query = await db.Table<Reserva>().Where(x => x.estadoid == 1 && x.estadoid == 2 && x.estadoid == 3 && x.estadoid == 5).ToListAsync();
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
