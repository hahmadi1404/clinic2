using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QCS_Config.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class IntroGalleryController : ControllerBase
{
    // GET: api/Intro/5
    [HttpGet("{clinicId}")]
    public async Task<ActionResult<List<string>>> GetIntroGallery(int clinicId)
    {
        var IntroGallery = new List<string>();

        
        return IntroGallery;
    }
}