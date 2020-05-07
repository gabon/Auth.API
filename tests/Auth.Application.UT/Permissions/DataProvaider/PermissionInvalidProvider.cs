﻿using Auth.Application.Permisions.Commands.AddPermissionInRole;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Auth.Application.UT.Common
{
    public class PermissionInvalidProvider : TheoryData<AddPermissionRoleCommand>
    {

        public PermissionInvalidProvider()
        {
            Add(new AddPermissionRoleCommand()
            {
                RoleName = "guest",
                Permisions = new Dictionary<string, IEnumerable<string>>()
               {
                   {"Api1", new List<string>(){"read" } }
               }
            });

            Add(new AddPermissionRoleCommand()
            {
                RoleName = "guest",
                Permisions = new Dictionary<string, IEnumerable<string>>()
               {
                   {"Api1", new List<string>(){"read" } },
                   {"auth.application", new List<string>()
                        {
                            AuthPermisions.RoleGet,
                            AuthPermisions.RoleSearch,
                            AuthPermisions.UserGet,
                            AuthPermisions.UserSearch
                        }
                   }
               }
            });
        }
    }
}