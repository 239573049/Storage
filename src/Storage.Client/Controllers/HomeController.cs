using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Storage.Client.Controllers;

[Route("/")]
[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet]
    public RedirectResult Get()
    {
        return new RedirectResult(url: "/index.html");
    }
}
