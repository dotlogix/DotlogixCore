// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Program.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Diagnostics;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Http.State;
using DotLogix.Core.Rest.Server.Routes;
using DotLogix.Core.Rest.Services.Attributes;
using DotLogix.Core.Rest.Services.Attributes.Events;
using DotLogix.Core.Rest.Services.Attributes.Http;
using DotLogix.Core.Rest.Services.Attributes.Routes;
using DotLogix.Core.Rest.Services.Base;
using DotLogix.Core.Rest.Services.Exceptions;
using DotLogix.Core.Rest.Services.Processors;
using DotLogix.Core.Security;
#endregion

namespace TestApp {
    internal class Program {
        private static void Main(string[] args) {
            var userGuid = Guid.NewGuid();
            var tokenGuid = Guid.NewGuid();

            var bytes = new byte[32];
            Array.Copy(userGuid.ToByteArray(), 0, bytes,0, 16);
            Array.Copy(tokenGuid.ToByteArray(), 0, bytes, 16, 16);
            
            var password = Encoding.UTF8.GetBytes("as#121293m");

            var encrypted = Encryption.EncryptAes(bytes, password, 1);
            var decrypted = Encryption.DecryptAes(encrypted, password);

            var buffer = new byte[16];
            Array.Copy(decrypted, 0, buffer, 0, 16);
            var decryptedUser = new Guid(buffer);
            Array.Copy(decrypted, 16, buffer, 0, 16);
            var decryptedToken = new Guid(buffer);


            Console.WriteLine($"Orig User: {userGuid}");
            Console.WriteLine($"Orig Token: {tokenGuid}");

            Console.WriteLine($"Payload: {Convert.ToBase64String(encrypted)}");

            Console.WriteLine($"Dec User: {decryptedUser}");
            Console.WriteLine($"Dec Token: {decryptedToken}");


            Console.Read();
        }
    }
}
