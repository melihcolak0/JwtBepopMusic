using Jwt.BusinessLayer.Abstract;
using Jwt.DtoLayer.UserDtos;
using Jwt.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _12PC_Jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetList()
        {
            var user = _userService.GetAll();
            return Ok(user);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            if (user == null)
                return NotFound("Paket bulunamadı!");

            return Ok(user);
        }      
             
        [HttpDelete("{id}")]
        public IActionResult DeletePackage(int id)
        {
            var user = _userService.GetById(id);
            if (user == null)
                return NotFound("Silinecek paket bulunamadı!");

            _userService.Delete(user);
            return Ok("Paket başarıyla silindi!");
        }

        [HttpGet("GetUserPackageIdByUserId/{id}")]
        public IActionResult GetUserPackageIdByUserId(int id)
        {
            var user = _userService.TGetUserPackageIdByUserId(id);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı!");

            return Ok(user);
        }

        [HttpGet("GetUserListWithPackages")]
        public IActionResult GetUserListWithPackages()
        {
            var users = _userService.TGetUserListWithPackages();

            var result = users.Select(u => new ResultUserWithPackagesDto
            {
                Id = u.Id,
                Username = u.Username,
                NameSurname = u.NameSurname,
                Email = u.Email,
                City= u.City,
                Role = u.Role,
                PackageId = u.Package.Id,
                PackageName = u.Package.Name                
            }).ToList();

            return Ok(result);
        }

        [HttpGet("GetTotalUserCount")]
        public IActionResult GetTotalUserCount()
        {
            int user = _userService.TGetTotalUserCount();
            return Ok(user);
        }
    }
}
