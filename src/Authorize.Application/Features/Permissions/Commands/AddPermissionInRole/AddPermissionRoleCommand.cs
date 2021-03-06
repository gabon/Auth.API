﻿using Authorize.Application.Attributtes;
using MediatR;
using System.Collections.Generic;

namespace Authorize.Application.Features.Permissions.Commands.AddPermissionInRole
{
    [Authorize(AuthPermissions.RoleCreated)]
    public class AddPermissionRoleCommand : IRequest
    {
        public string RoleName { get; set; }
        public Dictionary<string, IEnumerable<string>> Permisions { get; set; }

        public AddPermissionRoleCommand()
        {
            Permisions = new Dictionary<string, IEnumerable<string>>();
        }
    }
}
