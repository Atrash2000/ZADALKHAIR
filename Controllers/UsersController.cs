using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ZADALKHAIR.Data;
using ZADALKHAIR.Models;

namespace ZADALKHAIR.Controllers
{
    public class UsersController : Controller
    {
        private readonly ZADALKHAIRContext _context;

        public UsersController(ZADALKHAIRContext context)
        {
            _context = context;
        }
        [Route("Admin/ViewEmployee")]
        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpGet]
        [Route("Login")]
        
        public IActionResult Login(string? ReturnUrl)
        {
            Response.Cookies.Delete("UserLoginCookie", new CookieOptions()
            {
                Secure = true,
            });
            if (Url.IsLocalUrl(ReturnUrl))
                return RedirectToAction(nameof(Login));
            return View();
        }
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([Bind("Email, Password")] Login login)
        {
            if (ModelState.IsValid)
            {
                login.Password = ComputeStringToSha256Hash(login.Password);
                if (await UserLoginExists(login.Email, login.Password))
                {
                    var result = _context.User.Where(u => u.UserEmail == login.Email).SingleOrDefault();

                    var userClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, $"{result.UserLastName} {result.UserLastName}"),
                    new Claim(ClaimTypes.Email, result.UserEmail),
                    new Claim(ClaimTypes.Role, result.UserRoleType)
                 };

                    var userIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });
                    await HttpContext.SignInAsync(userPrincipal);

                    var tokenhandler = new JwtSecurityTokenHandler();
                    var tokenkey = Encoding.ASCII.GetBytes("[SECRET USED TO SIGN AND VERIFY JWT TOKENS, IT CAN BE ANY STRING]");
                    var tokendesciptor = new SecurityTokenDescriptor()
                    {
                        Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.PrimarySid, result.UserID.ToString()),
                    new Claim(ClaimTypes.Name, result.UserEmail),
                    new Claim(ClaimTypes.Role, result.UserRoleType)

                }),
                        Expires = DateTime.UtcNow.AddHours(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256Signature)

                    };
                    var token = tokenhandler.CreateToken(tokendesciptor);
                    TempData["token"] = tokenhandler.WriteToken(token);
                    return RedirectToAction("Dashboard", "Admin");
                }
            }
            return View(login);
        }

        [Route("Admin/AddEmployee")]
        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Admin/AddEmployee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserEmail,UserFirstName,UserLastName,UserPhoneNumber,USerCountryCode,UserRoleType,UserCreateAt,UserPassword")] User user)
        {
            if (ModelState.IsValid)
            {
                user.UserPassword = ComputeStringToSha256Hash(user.UserPassword);
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        private string ComputeStringToSha256Hash(string userPassword)
        {
            // Create a SHA256 hash from string   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Computing Hash - returns here byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(userPassword));

                // now convert byte array to a string   
                StringBuilder stringbuilder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    stringbuilder.Append(bytes[i].ToString("x2"));
                }
                return stringbuilder.ToString();
            }
        }

        // GET: Users/Edit/5
        [HttpGet]
        [Authorize]
        [Authorize(Roles = "Admin")]
        [Route("Admin/profile/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            /*ViewData["cookies"] = Request.Cookies["UserLoginCookie"].ToString();*/
            if (id == null)
            {
                return NotFound();
            }
            var user = await _context.User.FindAsync(id);
            if (user == null )
            {
                return NotFound();
            }
            
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,UserEmail,UserFirstName,UserLastName,UserPhoneNumber,USerCountryCode,UserRoleType,UserPassword")] User user)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user.UserPassword = ComputeStringToSha256Hash(user.UserPassword);
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Edit), id);
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }


            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserID == id);
        }
        private async Task<bool> UserLoginExists(string email, string password)
        {
            return await _context.User.AnyAsync(e => e.UserEmail == email && e.UserPassword == password);
        }
    }
}
