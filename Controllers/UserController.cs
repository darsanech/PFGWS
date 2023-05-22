using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PFGWS.Models;
using SQLite;
using Microsoft.VisualBasic.FileIO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PFGWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        string databasePath = Path.Combine(FileSystem.CurrentDirectory, "MyDataF.db");
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
        [Route("~/api/User/GetUbi")]
        [Authorize]
        public async Task<IEnumerable<User>> GetUbi()
        {
            var db = new SQLiteAsyncConnection(databasePath);
            var query = await db.QueryAsync<User>("select userid, ubicacion from Users");
            await db.CloseAsync();
            return query;
        }
        [HttpPut]
        [Route("~/api/User/PutUbi")]
        [Authorize]
        public async Task PutUbi(string newUbi)
        {
            if (newUbi != "")
            {
                var userid = Int32.Parse(User.FindFirst(ClaimTypes.Name).Value);
                var db = new SQLiteAsyncConnection(databasePath);
                User user = await db.Table<User>().FirstOrDefaultAsync(x => x.userid == userid);
                user.Ubicacion = newUbi;
                await db.UpdateAsync(user);
                await db.CloseAsync();
                await syncController.LoadData();

            }
        }

        [HttpGet]
        [Route("~/api/Login")]
        public async Task<IActionResult> Login(string usuario, string pass)
        {
            await syncController.LoadData();
            var db = new SQLiteAsyncConnection(databasePath);
            byte[] data = Encoding.ASCII.GetBytes(pass);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            String hash = System.Text.Encoding.ASCII.GetString(data);
            User user = await db.Table<User>().FirstOrDefaultAsync(x=>x.Username== usuario && x.Password==hash);
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
