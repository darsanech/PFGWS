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
    public class UserController : Controller
    {
        string databasePath = Path.Combine(FileSystem.CurrentDirectory, "MyDataRest.db");
        SyncController syncController = new SyncController();

        private IConfiguration _config;

        public UserController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<User>> Get()
        {
            var db = new SQLiteAsyncConnection(databasePath);
            var query = await db.Table<User>().ToListAsync();
            return query;
        }
        
        [HttpGet]
        [Route("~/api/Login")]
        public async Task<IActionResult> Login(string usuario, string pass)
        {
            await syncController.LoadData();
            var db = new SQLiteAsyncConnection(databasePath);

            User user = await db.Table<User>().FirstOrDefaultAsync(x=>x.Username== usuario && x.Password==pass);
            if (user == null)
            {
                return NotFound();
            }
            var expDate = _config.GetSection("JwtConfig").GetSection("expirationInMinutes").Value;
            var tc = new TokenController(_config);
            var token = tc.GenerarToken(user.userid.ToString(), user.Rol.ToString());
            var res = new BearerToken()
            {
                Token = token,
                ExpirationInMinutes = Int32.Parse(expDate),
                UserId = user.userid
            };
            return Ok(res);
        }
    }
}
