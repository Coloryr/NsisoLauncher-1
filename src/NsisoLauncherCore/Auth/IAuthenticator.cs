﻿using NsisoLauncherCore.Modules;
using NsisoLauncherCore.Net.MojangApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NsisoLauncherCore.Auth
{
    public interface IAuthenticator
    {
        Task<AuthenticateResult> DoAuthenticateAsync();
    }

    public class AuthenticateResult
    {
        public AuthState State { get; set; }

        public Error Error { get; set; }

        public string AccessToken { get; set; }

        public List<PlayerProfile> Profiles { get; set; }

        public PlayerProfile SelectedProfile { get; set; }

        public UserData UserData { get; set; }

        public AuthenticateResult(AuthState state)
        {
            this.State = state;
        }
    }

    public enum AuthState
    {
        SUCCESS,
        REQ_LOGIN,
        ERR_INVALID_CRDL,
        ERR_NOTFOUND,
        ERR_METHOD_NOT_ALLOW,
        ERR_OTHER,
        ERR_INSIDE
    }
}
