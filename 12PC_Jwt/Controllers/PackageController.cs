using Jwt.BusinessLayer.Abstract;
using Jwt.DtoLayer.PackageDtos;
using Jwt.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _12PC_Jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;

        public PackageController(IPackageService packageService)
        {
            _packageService = packageService;
        }
      
        [HttpGet]
        public IActionResult GetList()
        {
            var packages = _packageService.GetAll();
            return Ok(packages);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var package = _packageService.GetById(id);
            if (package == null)
                return NotFound("Paket bulunamadı!");

            return Ok(package);
        }

        [HttpPost]
        public IActionResult CreatePackage(CreatePackageDto createPackageDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var package = new Package
            {
                Name = createPackageDto.Name,
                Price = createPackageDto.Price,
                Description = createPackageDto.Description
            };

            _packageService.Insert(package);

            return Ok("Paket başarıyla eklendi!");
        }

        [HttpPut]
        public IActionResult UpdatePackage(UpdatePackageDto updatePackageDto)
        {
            var existingPackage = _packageService.GetById(updatePackageDto.Id);
            if (existingPackage == null)
                return NotFound("Güncellenecek paket bulunamadı!");

            existingPackage.Name = updatePackageDto.Name;
            existingPackage.Price = updatePackageDto.Price;
            existingPackage.Description = updatePackageDto.Description;

            _packageService.Update(existingPackage);

            return Ok("Paket başarıyla güncellendi!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePackage(int id)
        {
            var package = _packageService.GetById(id);
            if (package == null)
                return NotFound("Silinecek paket bulunamadı!");

            _packageService.Delete(package);
            return Ok("Paket başarıyla silindi!");
        }
    }
}
