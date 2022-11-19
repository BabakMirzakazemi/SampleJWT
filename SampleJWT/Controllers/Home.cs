using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DataLayer;
using SampleJWT.Models;
using SampleJWT.Services;
using System.Security.Cryptography;
using System.Text;

namespace SampleJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Home : ControllerBase
    {
        private readonly ProjectDbContext  _db;
        public Home(ProjectDbContext projectDbContext)
        {
           this._db = projectDbContext;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            var existUser = await _db.Tbl_Users.Where(u => u.UserName == registerModel.UserName).AnyAsync();
            if (existUser)
                return Conflict();
            var user = new User
            {
                Uid = Guid.NewGuid(),
                FullName = registerModel.FullName,
                UserName = registerModel.UserName,
                PasswordHash = GetHash(registerModel.Password),
                Role = registerModel.Role,
                IsActive = true
            };
            _db.Tbl_Users.Add(user);
            await _db.SaveChangesAsync();
            return Ok(true);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Login(string userName, string password)
        {
            var user = await _db.Tbl_Users.Where(u => u.UserName == userName).FirstOrDefaultAsync();
            if (user == null)
                return NotFound();
            if (user.PasswordHash != GetHash(password))
            {
                return Conflict();
            }
           // return Ok(user.Id);// zamani ke jew ra rah biandazim be jaye Id bayad Token ra bargardanim
            return Ok(GenerateToken.GetToken(user));//dar in ja yek token barmigardanim va be user tahvil midahim
        }

        
        [Authorize]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetCategory()
        {
            var category = await _db.Tbl_Categories.ToListAsync();
            return Ok(category);
        }


        [Authorize(Roles ="Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddCategory(CategoryModel cat)
        {
            var category = new Category
            {
                Title = cat.Title
            };
            await _db.Tbl_Categories.AddAsync(category);
            await _db.SaveChangesAsync();
            return Ok();
        }
        private string GetHash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
