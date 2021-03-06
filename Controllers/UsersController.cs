using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ZADALKHAIR.Data;
using ZADALKHAIR.Models;

namespace ZADALKHAIR.Controllers
{
    public class UsersController : Controller
    {
        private readonly ZADALKHAIRContext _context;
        private readonly IWebHostEnvironment webhostenvironment;

        public UsersController(ZADALKHAIRContext context, IWebHostEnvironment webhostenvironment)
        {
            _context = context;
            this.webhostenvironment = webhostenvironment;
        }

        [HttpGet]
        [Route("Admin/ViewEmployee")]
        [Authorize]
        [Authorize(Roles = "Admin, Employee")]
        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        [HttpGet]
        [Route("Admin/User/Details/injuction/{id}")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<PartialViewResult> Details(int? id)
        {
            if (id == null)
            {
                return PageNotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return PageNotFound();
            }
            TempData["AdminCount"] = _context.User.Where(u => u.UserRoleType == "Admin").Count();
            return PartialView("_UserDetails", user);
        }

        [HttpGet]
        [Route("Registration")]
        public IActionResult Registration()
        {
            return View();
        }

        /*[HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Registration([Bind("UserEmail,UserFirstName,UserLastName,UserPhoneNumber,USerCountryCode,UserRoleType,UserPassword,ProfilePic,UserProfilePic,UserCreateAt")] User user)
        {
            user.UserCreateAt = DateTime.Now;
            string profilePic = UploadFile(user.ProfilePic);
            user.UserProfilePic = profilePic;
            if (ModelState.IsValid)
            {
                user.UserPassword = ComputeStringToSha256Hash(user.UserPassword);
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            return View(user);
        }*/

        [HttpGet]
        [Route("Login")]
        public IActionResult Login(string? ReturnUrl)
        {
            Response.Cookies.Delete("UserLoginCookie", new CookieOptions()
            {
                Secure = true,
            });

            if (Url.IsLocalUrl(ReturnUrl))
            {
                return RedirectToAction(nameof(Login));
            }

            if (_context.User.Count() > 0)
            {
                return View();
            }
            else
            {
                return RedirectToAction(nameof(Registration));
            }
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
                        new Claim(ClaimTypes.PrimarySid, result.UserID.ToString()),
                        new Claim(ClaimTypes.Role, result.UserRoleType)
                    };

                    var userIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });
                    await HttpContext.SignInAsync(userPrincipal);

                    TempData["token"] = CreateToken(result).ToString();
                    return Redirect($"Admin/profile/{result.UserID}");
                }
            }
            return View(login);
        }

        private string CreateToken(User user)
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            var tokenkey = Encoding.ASCII.GetBytes("[SECRET USED TO SIGN AND VERIFY JWT TOKENS, IT CAN BE ANY STRING]");
            var tokendesciptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.PrimarySid, user.UserID.ToString()),
                    new Claim(ClaimTypes.Role, user.UserRoleType)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256Signature),
            };
            return tokenhandler.CreateEncodedJwt(tokendesciptor);
        }

        [Route("Admin/AddEmployee")]
        [HttpGet]
        [Authorize]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var user = new User()
            {
                UserEmail = ""
            };
            return View(user);
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Admin/AddEmployee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserEmail,UserFirstName,UserLastName,UserPhoneNumber,USerCountryCode,UserRoleType,UserPassword,ProfilePic,UserProfilePic,UserCreateAt")] User user)
        {
            user.UserCreateAt = DateTime.Now;
            string profilePic = UploadFile(user.ProfilePic);
            user.UserProfilePic = profilePic;
            if (ModelState.IsValid)
            {
                user.UserPassword = ComputeStringToSha256Hash(user.UserPassword);
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        private string UploadFile(IFormFile profilePic)
        {
            string fileName = null;

            if (profilePic != null)
            {
                string uploadDir = Path.Combine(webhostenvironment.WebRootPath, "images/profilePic");
                fileName = profilePic.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    profilePic.CopyTo(fileStream);
                }
            }
            return fileName;
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
        [Authorize(Roles = "Admin, Employee")]
        [Route("Admin/profile/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _context.User.FindAsync(id);
            if (user == null)
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
        [Route("Admin/profile/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,UserEmail,ProfilePic,UserFirstName,UserLastName,UserPhoneNumber,USerCountryCode,UserRoleType,UserPassword,UserProfilePic")] User user)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }
            user.UserCreateAt = DateTime.Now;
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

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(User user)
        {
            if (_context.User.Count() > 1)
            {
                var empInfo = await _context.User.FindAsync(user.UserID);
                _context.User.Remove(empInfo);
                await _context.SaveChangesAsync();
                /*TempData["Status"] = _context.SaveChangesAsync().IsFaulted;*/
            }
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

        [HttpGet]
        public PartialViewResult PageNotFound()
        {
            return PartialView("_PageNotFound");
        }
    }
}
