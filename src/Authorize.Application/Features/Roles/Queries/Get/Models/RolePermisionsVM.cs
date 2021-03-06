﻿using Authorize.Application.Features.Roles.Queries.Models;
using System.Collections.Generic;

namespace Authorize.Application.Features.Roles.Queries.Get.Models
{
    public class RolePermissionsVM : RoleVM
    {
        public RolePermissionsVM(string name, string description, bool isEnabled,
            IDictionary<string, IEnumerable<string>> permissions)
            : base(name, description, isEnabled)
        {
            Permissions = permissions ?? new Dictionary<string, IEnumerable<string>>();
        }

        public IDictionary<string, IEnumerable<string>> Permissions { get; set; }


    }
}
