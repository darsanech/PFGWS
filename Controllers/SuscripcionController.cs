using Microsoft.AspNetCore.Mvc;
using PFGWS.Models;
using Microsoft.VisualBasic.FileIO;
using SQLite;
using System.Security.Claims;

namespace PFGWS.Controllers
{
    public class SuscripcionController : ControllerBase
    {
        string databasePath = Path.Combine(FileSystem.CurrentDirectory, "MyData.db");

        [HttpPost]
        public async void Post(int campid)
        {
            var db = new SQLiteAsyncConnection(databasePath);
            Suscripcion nsub= new Suscripcion();
            nsub.campingid = campid;
            nsub.userid = Int32.Parse(User.FindFirst(ClaimTypes.Name).Value);
            nsub.update = true;
            await db.InsertAsync(nsub);
            await db.CloseAsync();
        }
        [HttpPost]
        [Route("~/api/Suscripcion/NewUpdate")]
        public async void NewUpdate(int campid)
        {
            var db = new SQLiteAsyncConnection(databasePath);
            var userid = User.FindFirst(ClaimTypes.Name).Value;
            var query0 = await db.QueryAsync<Suscripcion>("update Suscripcion set update=1 " +
                "where campingid=" + campid+" AND userid!="+userid);
            await db.CloseAsync();
        }
        [HttpGet]
        public async Task<bool> Get(int campid)
        {
            var db = new SQLiteAsyncConnection(databasePath);
            var userid = Int32.Parse(User.FindFirst(ClaimTypes.Name).Value);
            Suscripcion sus = await db.Table<Suscripcion>().FirstOrDefaultAsync(x => x.userid == userid && x.campingid==campid);
            await db.CloseAsync();
            return sus.update;
        }
        [HttpPost]
        [Route("~/api/Suscripcion/GetAll")]

        public async Task<IEnumerable<Suscripcion>> GetAll()
        {
            var db = new SQLiteAsyncConnection(databasePath);
            var userid = Int32.Parse(User.FindFirst(ClaimTypes.Name).Value);
            var query = await db.Table<Suscripcion>().Where(x => x.userid == userid).ToListAsync();
            await db.CloseAsync();
            return query;
        }
    }
}
