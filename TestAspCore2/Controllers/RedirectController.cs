namespace TestAspCore2.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using NSwag.Annotations;

    [OpenApiIgnore]
    public class RedirectController : ControllerBase
    {
        [HttpGet]
        [Route("/")]
        public IActionResult Get() => this.Redirect("/swagger");
    }
}