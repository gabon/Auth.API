﻿using Authorize.Application.Contracts;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Authorize.API.REST.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserName = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? 
                httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name)
#if DEBUG
                ??
                "admin";
#else
            ;
#endif
        }

        public string UserName { get; }
    }
}
