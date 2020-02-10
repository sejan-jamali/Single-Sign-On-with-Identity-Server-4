﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace single_sign_on_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        [Route("getdata")]
        public ActionResult Get()
        {
            var claims = HttpContext.User.Claims.Select(x => $"{x.Type}:{x.Value}");
            return Ok(new
            {
                Name = "Values API",
                Claims = claims.ToArray()
            });
        }
    }
}