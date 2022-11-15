using Login.DataContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Login.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly ILogger<BaseController> _logger;
        protected ApplicationContext _db;

        public BaseController(ILogger<BaseController> logger, ApplicationContext context)
        {
            _logger = logger;
            _db= context;
        }
    }
}
