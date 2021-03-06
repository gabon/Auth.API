﻿using Authorize.Application.Exceptions;
using Authorize.Application.Features.Roles.Commands.Create;
using Authorize.Application.UT.Common;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace Authorize.Application.UT.Roles.Commands
{
    [ExcludeFromCodeCoverage]
    public class CreateRoleTest : BaseTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam sed tincidunt magna, ac consequat mauris. Praesent turpis augue, laoreet sed justo ut, efficitur euismod tortor. Ut laoreet nec ex nunc asdsdasdas das asdasdasdasdas")]
        public async Task When_CreateRole_InputInValid_ThrowValidationException(string roleName)
        {
            var mediator = ServiceProvider.GetService<IMediator>();
            Func<Task> act = async () =>
            {
                await mediator.Send(new CreateRoleCommand()
                {
                    Name = roleName
                });
            };
            act.Should().Throw<ValidationException>();

        }

        [Theory]
        [InlineData(Constants.RoleAdmin + "_test")]
        [InlineData(Constants.RoleGuest + "_test")]
        public async Task When_CreateRole_InputIsValid_Return(string roleName)
        {
            using var scope = ServiceScopeProvider.CreateScope();
            var sp = scope.ServiceProvider;
            var mediator = sp.GetService<IMediator>();
            //Act
            var response = await mediator.Send(new CreateRoleCommand()
            {
                Name = roleName
            });
            //Assert
            response.Should().NotBeNull();

        }

        [Theory]
        [InlineData(Constants.RoleAdmin)]
        [InlineData(Constants.RoleGuest)]
        public async Task When_CreateRole_InputIsValid_ThrowExistsException(string roleName)
        {
            using var scope = ServiceScopeProvider.CreateScope();
            var sp = scope.ServiceProvider;

            var mediator = sp.GetService<IMediator>();
            //Act
            Func<Task> act = async () =>
            {
                await mediator.Send(new CreateRoleCommand()
                {
                    Name = roleName
                });
            };
            //Assert
            act.Should().Throw<ExistsException>();

        }
    }
}
