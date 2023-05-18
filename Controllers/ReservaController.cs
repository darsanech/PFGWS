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
        SyncController syncController=new SyncController();
        SuscripcionController susController=new SuscripcionController();
        [HttpPost]

        public async void Post([FromBody] Reserva reserva)
        {
            var userid = Int32.Parse(User.FindFirst(ClaimTypes.Name).Value);
            var db = new SQLiteAsyncConnection(databasePath);
            await db.InsertAsync(reserva);
            await db.CloseAsync();
            //await syncController.LoadData();
            await susController.UpdateThem1(reserva.campingid,userid);
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
        [Route("/api/Reserva/GetCamping")]
        public async Task<IEnumerable<Reserva>> GetUser(int campid)
        {
            var db = new SQLiteAsyncConnection(databasePath);
            var query = await db.Table<Reserva>().Where(x => x.campingid==campid).ToListAsync();
            await db.CloseAsync();
            var userid = Int32.Parse(User.FindFirst(ClaimTypes.Name).Value);
            await susController.UpdateMe0(campid,userid);
            return query;
        }
        [HttpPut]
        public async void Put([FromBody] Reserva reserva)
        {
            var userid = Int32.Parse(User.FindFirst(ClaimTypes.Name).Value);
            var db = new SQLiteAsyncConnection(databasePath);
            await db.UpdateAsync(reserva);
            await db.CloseAsync();
            await susController.UpdateThem1(reserva.campingid, userid);
            await syncController.LoadData();
        }
    }
}
