using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using PFGWS.Models;
using SQLite;
using System.Security.Claims;

namespace PFGWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaProductoController : Controller
    {
        string databasePath = Path.Combine(FileSystem.CurrentDirectory, "MyDataFFF.db");

        [HttpGet]
        public async Task<IEnumerable<ReservaProducto>> Get()
        {
            var db = new SQLiteAsyncConnection(databasePath);
            var query = await db.Table<ReservaProducto>().ToListAsync();
            await db.CloseAsync();
            return query;
        }
        [HttpGet]
        [Route("/api/ReservaProducte/Test")]
        public async Task<IEnumerable<Test>> Test()
        {
            var db = new SQLiteAsyncConnection(databasePath);
            var query = await db.QueryAsync<Test>(
                @"select r.idreserva, GROUP_CONCAT(p.productoname, ' x', rp,quantitat) json
                from Reserva r inner join reservaproducto rp on r.idreserva=rp.idreserva inner join Producto p on rp.producteid=p.producteid group by r.idreserva");

            await db.CloseAsync();
            return query;
        }
    }
}