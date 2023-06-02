using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PFGWS.Models;
using SQLite;
using Microsoft.VisualBasic.FileIO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;

namespace PFGWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : Controller
    {
        string databasePath = Path.Combine(FileSystem.CurrentDirectory, "MyDataFFF.db");
        SyncController syncController=new SyncController();
        SuscripcionController susController=new SuscripcionController();
        ReservaProductoController rpController=new ReservaProductoController();
        ParcelaController parController=new ParcelaController();



        [HttpPost]

        public async Task<int> Post([FromBody] Newtonsoft.Json.Linq.JObject data)
        {
            Reserva nReserva = data["nReserva"].ToObject<Reserva>();
            List<ReservaProducto> nReservaProducto = data["nReservaProducto"].ToObject<List<ReservaProducto>>();
            var userid = Int32.Parse(User.FindFirst(ClaimTypes.Name).Value);
            var db = new SQLiteAsyncConnection(databasePath);
            var i=await db.InsertAsync(nReserva);
            await db.CloseAsync();
            nReservaProducto.ToList().ForEach(x => x.idreserva = nReserva.idreserva);
            await rpController.Post(nReservaProducto,nReserva.estadoid);
            await parController.Put(nReserva.campingid, nReserva.numeroparcela, nReserva.estadoid);
            await susController.UpdateThem1(nReserva.campingid,userid);
            await syncController.LoadData();
            return nReserva.idreserva;
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
        [Route("/api/Reserva/GetEstado")]
        public async Task<int> GetEstado(int idreserva)
        {
            var db = new SQLiteAsyncConnection(databasePath);
            var res = await db.Table<Reserva>().Where(x=>x.idreserva==idreserva).FirstOrDefaultAsync();
            await db.CloseAsync();
            return res.estadoid;
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
        public async Task Put([FromBody] Newtonsoft.Json.Linq.JObject data)
        {
            Reserva nReserva = data["nReserva"].ToObject<Reserva>();
            List<ReservaProducto> nReservaProducto = data["nReservaProducto"].ToObject<List<ReservaProducto>>();
            var userid = Int32.Parse(User.FindFirst(ClaimTypes.Name).Value);
            var db = new SQLiteAsyncConnection(databasePath);

            Reserva oldReserva = await db.Table<Reserva>().Where(x => x.idreserva == nReserva.idreserva).FirstOrDefaultAsync();
            List<ReservaProducto> oldReservaProducto = await db.Table<ReservaProducto>().Where(x => x.idreserva == nReserva.idreserva).ToListAsync();

            await rpController.Put(nReservaProducto, oldReservaProducto, nReserva.estadoid, oldReserva.estadoid);

            await db.UpdateAsync(nReserva);
            await db.CloseAsync();
            await parController.Put(nReserva.campingid, nReserva.numeroparcela, nReserva.estadoid);
            await susController.UpdateThem1(nReserva.campingid, userid);
            await syncController.LoadData();
        }
        [HttpGet]
        [Route("/api/ReservaProducto/GetReservaProductos")]
        public async Task<IEnumerable<ReservaProducto>> GetReservaProductos(int idreserva)
        {
            await syncController.LoadData();
            var db = new SQLiteAsyncConnection(databasePath);
            var query = await db.Table<ReservaProducto>().Where(x=>x.idreserva==idreserva).ToListAsync();
            await db.CloseAsync();
            return query;
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
                await parController.Put(item.campingid, item.numeroparcela, 3);
            }
            await susController.UpdateThem1(-1, userid);
            await syncController.LoadData();
            return query.Count();
        }
        [HttpGet]
        [Route("/api/Reserva/SetEntregar")]
        public async Task<int> SetEntregar()
        {
            var userid = Int32.Parse(User.FindFirst(ClaimTypes.Name).Value);
            var db = new SQLiteAsyncConnection(databasePath);
            string now = DateTime.Now.ToString("dd/MM/yyyy");
            var query = await db.Table<Reserva>().Where(x => x.estadoid == 7 && x.datainici == now).ToListAsync();
            await db.CloseAsync();
            foreach (var item in query)
            {
                item.estadoid = 1;
                await db.UpdateAsync(item);
                await parController.Put(item.campingid, item.numeroparcela, 1);
            }
            await susController.UpdateThem1(-1, userid);
            await syncController.LoadData();
            return query.Count();
        }
    }
}
