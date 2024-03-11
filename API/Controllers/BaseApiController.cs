using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
//This route is used by the API to know where to listen for API requests. This still works the same as MVC where it listens to WeatherForecast without the controller part.
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{

}
