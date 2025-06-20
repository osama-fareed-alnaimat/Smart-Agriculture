﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SmartAgriculture.Domain.Entities;
using SmartAgriculture.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartAgriculture.Application.Users.Commands.UpdateUserDetails
{
    public class UpdateUserDetailsCommandHandler(ILogger<UpdateUserDetailsCommandHandler> logger,
        IUserContext userContext,
        IUserStore<User> userStore) : IRequestHandler<UpdateUserDetailsCommand>
    {
        public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
        {
            var user = userContext.GetCurrentUser();

            logger.LogInformation("Updating user: {UserId} with {@Requset}", user!.Id, request);

            var dbUser = await userStore.FindByIdAsync(user!.Id, cancellationToken);

            if (dbUser == null)
                throw new NotFoundException(nameof(User), user!.Id);

            dbUser.Nationality = request.Nationality;
            dbUser.DateOfBirth = request.DateOfBirth;
            dbUser.FirstName = request.FirstName;
            dbUser.LastName = request.LastName;
            dbUser.Email = request.Email;
            dbUser.UserName = request.Email;
            dbUser.NormalizedEmail = request.Email.ToUpperInvariant();
            dbUser.NormalizedUserName = request.Email.ToUpperInvariant();
            

            await userStore.UpdateAsync(dbUser, cancellationToken);
        }
    }
}
