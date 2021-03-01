﻿using CommunityBoard.Core.DomainObjects;
using CommunityBoard.Core.Models;
using System;
using System.Threading.Tasks;

namespace CommunityBoard.Core.Interfaces
{
    public interface IIdentityRepository
    {
        Task<AuthenticationResult> RegisterAsync(
            string firstName, string lastName, string userName, string email, string password);
        Task<AuthenticationResult> LoginAsync(string emailOrUserName, string password);
    }
}