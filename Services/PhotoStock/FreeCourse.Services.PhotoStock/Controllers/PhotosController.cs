using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {

        [HttpPost]
        public Task<IActionResult> PhotoSave(IFormFile photo,CancellationToken cancellationToken)//cancellation token koyma sebebimiz
        {

        }
    }
}
