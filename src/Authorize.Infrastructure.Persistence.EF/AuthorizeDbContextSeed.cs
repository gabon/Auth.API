﻿using Authorize.Application;
using Authorize.Application.Contracts;
using Authorize.Domain.Applications;
using Authorize.Domain.Roles;
using Authorize.Domain.Users;
using System.Collections.Generic;
using System.Linq;

namespace Authorize.Infrastructure.Persistence.EF
{
    public static class AuthorizeDbContextSeed
    {       

        public static void SeedDefaultAsync(IAppDbContext context, IAuthPermissions authPermissions)
        {
            var appAuth = SeedApplication(context, authPermissions);
            SeedUserDefault(context, appAuth);
            SeedApiRegister(context, appAuth);
        }
       

        private static Authorize.Domain.Applications.Application SeedApplication(IAppDbContext context, IAuthPermissions authPermissions)
        {
            var app = new Authorize.Domain.Applications.Application("authorize.application")
            {
                Permissions = authPermissions.Permissions.ToList()
            };
            context.Applications.Add(app);
            return app;
        }

        private static void SeedUserDefault(IAppDbContext context, Authorize.Domain.Applications.Application appAuth)
        {
            var defaultRole = new Role("admin", "admin");
            var defaultUser = new User("admin");

            defaultRole.Applications.Add(new ApplicationRole()
            {
                Application = appAuth,
                Permissions = appAuth.Permissions

            });
            defaultRole.Users.Add(new Domain.Relations.UserRole()
            {
                User = defaultUser,
                Role = defaultRole
            });


            context.Users.Add(defaultUser);
            context.Roles.Add(defaultRole);
        }

        private static void SeedApiRegister(IAppDbContext context, Authorize.Domain.Applications.Application appAuth)
        {
            var defaultRole = new Role("ApiRegister", "This role can register a new api");
            var defaultUser = new User("ApiRegister");

            defaultRole.Applications.Add(new ApplicationRole()
            {
                Application = appAuth,
                Permissions = new List<Permission>()
                {
                    Permission.For(AuthPermissions.ApplicationCreated)
                }

            });
            defaultRole.Users.Add(new Domain.Relations.UserRole()
            {
                User = defaultUser,
                Role = defaultRole
            });


            context.Users.Add(defaultUser);
            context.Roles.Add(defaultRole);
        }
    }
}
