using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Binxio.Abstractions;
using Binxio.Common.Projects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Binxio.Management.Web.Controllers
{
    [Route("api/project")]
    [ApiController]
    public class ProjectApiController : XioApiController
    {
        private readonly IProjectManager pm;

        public ProjectApiController(IProjectManager pm)
        {
            this.pm = pm;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectCreateModel model) => ApiStatusResult(await pm.Create(model));
    }
}
