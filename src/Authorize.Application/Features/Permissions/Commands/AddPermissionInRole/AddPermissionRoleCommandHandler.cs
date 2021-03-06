﻿using Authorize.Application.Contracts;
using Authorize.Application.Exceptions;
using Authorize.Application.Validators;
using Authorize.Domain.Applications;
using Authorize.Domain.Roles;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Authorize.Application.Features.Permissions.Commands.AddPermissionInRole
{
    public class AddPermissionRoleCommandHandler : IRequestHandler<AddPermissionRoleCommand>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _cuserService;
        public AddPermissionRoleCommandHandler(IAppDbContext context, ICurrentUserService cuserService)
        {
            _context = context;
        }
        public async Task<Unit> Handle(AddPermissionRoleCommand command, CancellationToken cancellationToken)
        {
            var entity = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == command.RoleName, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(Role), command.RoleName);
            }
            await new PermissionsExistsValidator(_context.Applications, command.Permisions, cancellationToken)
                .ValidAsync();
            foreach (var app in command.Permisions)
            {
                cancellationToken.ThrowIfCancellationRequested();
                entity.Applications.Add(new ApplicationRole()
                {
                    Application = new Domain.Applications.Application(app.Key),
                    Permissions = app.Value.Select(p => Permission.For(p)).ToList()
                });
            }
            cancellationToken.ThrowIfCancellationRequested();
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
