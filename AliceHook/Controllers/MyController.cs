using Microsoft.AspNetCore.Mvc;

namespace AliceHook.Controllers
{
    [ApiController]
    [Route("/")]
    public class MyController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "It works!";
        }
    }
}