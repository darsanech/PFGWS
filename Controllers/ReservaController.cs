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
        string databasePath = Path.Combine(FileSystem.CurrentDirectory, "MyDataF.db");
        SyncController syncController=new SyncController();
        SuscripcionController susController=new SuscripcionController();
        ParcelaController parController=new ParcelaController();
        ProductoController proController=new ProductoController();



        [HttpPost]

        public async Task<int> Post([FromBody] Reserva reserva)
        {
            var userid = Int32.Parse(User.FindFirst(ClaimTypes.Name).Value);
            var db = new SQLiteAsyncConnection(databasePath);
            var i=await db.InsertAsync(reserva);
            List<Reserva> listR = new List<Reserva>
            {
                reserva
            };
            await proController.MegaPut(listR);
            await db.CloseAsync();
            await parController.Put(reserva.campingid, reserva.numeroparcela, reserva.estadoid);
            await susController.UpdateThem1(reserva.campingid,userid);
            await syncController.LoadData();
            return reserva.idreserva;
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
        public async Task Put([FromBody] Reserva reserva)
        {
            var userid = Int32.Parse(User.FindFirst(ClaimTypes.Name).Value);
            var db = new SQLiteAsyncConnection(databasePath);
            Reserva oldreserva = await db.Table<Reserva>().FirstOrDefaultAsync(x => x.idreserva == reserva.idreserva);
            List<Reserva> listR = new List<Reserva>
            {
                reserva,
                oldreserva
            };
            await proController.MegaPut(listR);

            await db.UpdateAsync(reserva);
            await db.CloseAsync();
            await parController.Put(reserva.campingid, reserva.numeroparcela, reserva.estadoid);
            await susController.UpdateThem1(reserva.campingid, userid);
            await syncController.LoadData();
        }
        [HttpGet]
        [Route("/api/Reserva/SetRecoger")]
        public async Task<int> SetRecoger()
        {
            var userid = Int32.Parse(User.FindFirst(ClaimTypes.Name).Value);
            var db = new SQLiteAsyncConnection(databasePath);
            string now = DateTime.Now.ToString("dd/MM/yyyy");
            var query = await db.Table<Reserva>().Where(x => x.estadoid==2 && x.datafinal==now).ToListAsync();
            await db.CloseAsync();
            foreach(var item in query)
            {
                item.estadoid = 3;
                await db.UpdateAsync(item);
            }
            await susController.UpdateThem1(-1, userid);
            await syncController.LoadData();
            return query.Count();
        }
    }
}
