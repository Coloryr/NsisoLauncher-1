﻿using Nsisnamespace NsisoLauncherCore.Net.Apis.Modules.Yggdrasil;
using Nsisnamespace NsisoLauncherCore.Net.Apis.Modules.Yggdrasil.Requests;
using Nsisnamespace NsisoLauncherCore.Net.Apis.Modules.Yggdrasil.Responses;
using NsisoLauncherCore.Net;
using NsisoLauncherCore.Net.Yggdrasil;
using System;
using System.Threading.Tasks;

namespace NsisoLauncherCore.Auth
{
    public class Nide8Authenticator : YggdrasilAuthenticator
    {
        public string Nide8ID { get; set; }

        private Net.Nide8API.APIHandler nide8Handler;

        public Nide8Authenticator(NetRequester requester, string nide8ID) : base(
            string.Format("https://auth2.nide8.com:233/{0}/authserver", nide8ID), requester)
        {
            nide8Handler = new Net.Nide8API.APIHandler(requester, nide8ID);
        }

        public async Task UpdateApiRoot()
        {
            var result = await nide8Handler.GetInfoAsync();
            this.AuthServerUrl = result.APIRoot;
        }

        public async Task<Net.Nide8API.APIModules> GetInfo()
        {
            return await nide8Handler.GetInfoAsync();
        }

        public async Task<bool> TestIsNide8DnsOK()
        {
            return await nide8Handler.TestIsNide8DnsOK();
        }
    }
}
