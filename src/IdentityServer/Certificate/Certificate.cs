using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace IdentityServer.Certificates
{
    /// <summary>
    /// Helper to create a X509 Certificate
    /// </summary>
    static class Certificate
    {
        const string CERT_SECRET_NAME = "identityservercert";
        const string CERT_PWD_SECRET_NAME = "identityservercertpwd";

        /// <summary>
        /// Generates a X509 certificate
        /// </summary>
        /// <param name="config">App Configuration</param>
        /// <returns>X509 Certificate</returns>
        public static X509Certificate2 Get(IConfiguration config)
        {
           var certByteArray = Convert.FromBase64String(config[CERT_SECRET_NAME]);
           return new X509Certificate2(certByteArray, config[CERT_PWD_SECRET_NAME], X509KeyStorageFlags.MachineKeySet);
        }  
    }
}