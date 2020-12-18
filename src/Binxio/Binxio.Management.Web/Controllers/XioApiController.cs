using Binxio.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Binxio.Management.Web.Controllers
{
    public class XioApiController : ControllerBase
    {
        public IActionResult ApiStatusResult(XioResult result)
        {
            if (result.Status == ResultStatus.Failed)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
