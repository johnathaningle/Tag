using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TagConsumer.Data;
using TagConsumer.Models;
using System.Text;
using TagConsumer.Services;
using Microsoft.Extensions.Logging;

namespace TagConsumer.Controllers
{
    public class AuthController : Controller
    {
        private DataContext _context { get; set; }
        private ILogger<AuthController> _logger { get; set; }
        public AuthController(ILogger<AuthController> logger, DataContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public JsonResult Index()
        {
            return new JsonResult(new { name = "auth api" });
        }

        [HttpPost]
        public async Task<JsonResult> RegisterUser(string username, string password)
        {
            var crypto = new CryptoService();
            password = crypto.EncryptString(password);
            var user = new User
            {
                Username = username,
                Password = password
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync().ConfigureAwait(true);
            return new JsonResult(new { success = true });
        }

        [HttpGet]
        public async Task<JsonResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return new JsonResult(users);
        }

        [HttpPost]
        public async Task<JsonResult> RemoveUser(string username, string password)
        {
            var user = await _context.Users
                .Where(x => x.Username == username)
                .FirstOrDefaultAsync();

            if (user == null)
                return new JsonResult(new { success = false });

            var crypto = new CryptoService();
            var decrypted = crypto.DecryptString(user.Password);
            //check to make sure both passwords are the same
            if (decrypted == password)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return new JsonResult(new { success = true });
            }


            return new JsonResult(new { success = false });
        }
    }
}