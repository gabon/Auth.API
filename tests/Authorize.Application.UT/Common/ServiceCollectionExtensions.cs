﻿using Authorize.Application.Contracts;
using Authorize.Domain.Applications;
using Authorize.Domain.Relations;
using Authorize.Domain.Roles;
using Authorize.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MockQueryable.NSubstitute;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Authorize.Application.UT.Common
{
    internal class Constants
    {
        public const string App = "authorize.application";
        public const string UserAdmin = "admin";
        public const string UserGuest = "guest";
        public const string RoleAdmin = "admin";
        public const string RoleGuest = "guest";
    }

    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddMocks(this IServiceCollection service)
        {
            return service
            .AddSingleton<ICurrentUserService>(sp =>
            {
                var curs = Substitute.For<ICurrentUserService>();
                curs.UserName.Returns(Constants.UserAdmin);
                return curs;
            })
            .AddDBSetMocks<Authorize.Domain.Applications.Application>(sp =>
            {
                var authPermissions = sp.GetService<IAuthPermissions>();
                var data = new List<Authorize.Domain.Applications.Application>
                {
                    new Authorize.Domain.Applications.Application(Constants.App)
                    {
                        Permissions = authPermissions.Permissions.ToList()
                    }
                };
                return data;
            })
            .AddDBSetMocks<User>(sp =>
            {
                var username = sp.GetService<ICurrentUserService>().UserName;
                var roles = sp.GetService<DbSet<Role>>();
                var data = new List<User>()
                {
                    new User(Constants.UserAdmin, roles.ToList()),
                    new User(Constants.UserGuest, roles.Where(r=> r.Name == Constants.RoleGuest).ToList())
                };
                return data;
            })
            .AddDBSetMocks<Role>(sp =>
            {
                var applications = sp.GetService<DbSet<Authorize.Domain.Applications.Application>>();
                var authPermissions = sp.GetService<IAuthPermissions>();
                var users = new List<UserRole>
                {
                    new UserRole()
                    {
                        User =new User(Constants.UserAdmin)
                    }
                };
                var data = new List<Role>()
                {
                    new Role(Constants.RoleAdmin,"admin desc", applications.Select(a => new ApplicationRole(){Application = a,Permissions=authPermissions.Permissions.ToList() }).ToList())
                    {
                        Users = users
                    },
                    new Role(Constants.RoleGuest,"guest desc", applications.Select(a => new ApplicationRole(){ Application =a,Permissions= new List<Permission>()
                    {
                        Permission.For(AuthPermissions.RoleGet),
                        Permission.For(AuthPermissions.RoleSearch),
                        Permission.For(AuthPermissions.UserGet),
                        Permission.For(AuthPermissions.UserSearch)
                    } }).ToList())
                    {
                        Users = new List<UserRole>
                        {
                            new UserRole()
                            {
                                User = new User(Constants.UserGuest)
                            }
                        }
                    }
                };



                return data;
            })
            .AddScoped<IAppDbContext>(sp =>
            {
                var dbContext = Substitute.For<IAppDbContext>();
                var roles = sp.GetService<DbSet<Role>>();
                dbContext.Roles.Returns(roles);
                var users = sp.GetService<DbSet<User>>();
                dbContext.Users.Returns(users);
                var apps = sp.GetService<DbSet<Authorize.Domain.Applications.Application>>();
                dbContext.Applications.Returns(apps);
                return dbContext;
            });
        }
        public static IServiceCollection AddDBSetMocks<T>(this IServiceCollection service, Func<IServiceProvider, IEnumerable<T>> fakeData)
                    where T : class
        {
            return service.AddScoped<DbSet<T>>(sp =>
            {
                var dataFake = fakeData(sp);
                var mock = dataFake.AsQueryable().BuildMockDbSet();
                return mock;
            });
        }
    }
}