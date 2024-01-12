using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers;

[ApiController]
[Route("errors/{code}")]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : Controller
{
    public IActionResult Error(int code)
    {
        return new ObjectResult(new ApiResponse(code));
    }
}