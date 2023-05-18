using Microsoft.AspNetCore.Mvc;
using PFGWS.Models;
using Microsoft.VisualBasic.FileIO;
using SQLite;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace PFGWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SuscripcionController : ControllerBase
    {
        string databasePath = Path.Combine(FileSystem.CurrentDirectory, "MyDataA.db");
        SyncController syncController = new SyncController();

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
            await syncController.LoadData();

        }
        [HttpDelete]
        public async void Delete(int campid)
        {
            var db = new SQLiteAsyncConnection(databasePath);
            Suscripcion nsub = new Suscripcion();
            nsub.campingid = campid;
            nsub.userid = Int32.Parse(User.FindFirst(ClaimTypes.Name).Value);
            await db.DeleteAsync(nsub);
            await db.CloseAsync();
            await syncController.LoadData();

        }
        [HttpPut]
        [Route("~/api/Suscripcion/UpdateThem1")]

        public async Task UpdateThem1(int campid,int userid)
        {
            var db = new SQLiteAsyncConnection(databasePath);
            var query0 = await db.QueryAsync<Suscripcion>("update Suscripcion set needupdate=1 " +
                "where campingid=" + campid+" AND userid!="+userid);
            await db.CloseAsync();
            await syncController.LoadData();

        }
        [HttpPut]
        [Route("~/api/Suscripcion/UpdateMe0")]

        public async Task UpdateMe0(int campid, int userid)
        {
            var db = new SQLiteAsyncConnection(databasePath);
            var query0 = await db.QueryAsync<Suscripcion>("update Suscripcion set needupdate=0 " +
                "where campingid=" + campid + " AND userid=" + userid);
            await db.CloseAsync();
            await syncController.LoadData();

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
        [HttpGet]
        [Route("~/api/Suscripcion/GetMine")]

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
