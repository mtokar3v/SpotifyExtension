﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SpotifyExtension.DataItems.Config;
using SpotifyExtension.Interfaces.Services;

namespace SpotifyExtension.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        OAuthOptions _authOptions;
        IAuthorizeService _authorizeService;
        public AuthController(
            IOptions<OAuthOptions> authOptions,
            IAuthorizeService authorizeService
            )
        {
            _authOptions = authOptions.Value;
            _authorizeService = authorizeService;
        }

        [HttpGet]
        [Route(nameof(GetAuthLink))]
        public IActionResult GetAuthLink()
        {
            var authLink = _authorizeService.CreateAuthLink(nameof(GetCallback), _authOptions.Client.Id);
            return Ok(authLink);
        
        }

        [HttpGet]
        [Route(nameof(GetCallback))]
        public async Task<IActionResult> GetCallback(string code)
        {
            if (string.IsNullOrEmpty(code)) return BadRequest();

            var success = await _authorizeService.TryGetAccessAsync(nameof(GetCallback), code, HttpContext);
            if (!success) return Forbid();

            return Ok();
        }
    }
}