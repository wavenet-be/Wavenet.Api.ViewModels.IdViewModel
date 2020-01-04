// <copyright file="MachineKeyProtectionProvider.cs" company="Wavenet">
// Copyright (c) Wavenet. All rights reserved.
// </copyright>

namespace Wavenet.Api.ViewModels.ProtectionProviders
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
#if NETFRAMEWORK
    using System.Web.Security;
#else
    using Microsoft.AspNetCore.DataProtection;
#endif

    /// <summary>
    /// <see cref="MachineKeyProtectionProvider"/>.
    /// </summary>
    /// <seealso cref="IIdViewModelProtectionProvider" />
    public class MachineKeyProtectionProvider : IIdViewModelProtectionProvider
    {
#if NETCOREAPP
        /// <summary>
        /// The data protector.
        /// </summary>
        private readonly IDataProtector dataProtector;

        /// <summary>
        /// Initializes a new instance of the <see cref="MachineKeyProtectionProvider"/> class.
        /// </summary>
        /// <param name="dataProtectionProvider">The data protection provider.</param>
        public MachineKeyProtectionProvider(IDataProtectionProvider dataProtectionProvider)
        {
            this.dataProtector = dataProtectionProvider.CreateProtector(nameof(IdViewModel));
        }
#endif

        /// <summary>
        /// Protects the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The protected id.
        /// </returns>
        public string Protect(int id)
#if NETFRAMEWORK
            => ToWebSafeBase64(MachineKey.Protect(BitConverter.GetBytes(id), nameof(IdViewModel)));
#else
            => ToWebSafeBase64(this.dataProtector.Protect(BitConverter.GetBytes(id)));
#endif

        /// <summary>
        /// Unprotects the specified code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>
        /// The underlying id.
        /// </returns>
        public int Unprotect(string code)
#if NETFRAMEWORK
            => BitConverter.ToInt32(MachineKey.Unprotect(FromWebSafeBase64(code), nameof(IdViewModel)), 0);
#else
            => BitConverter.ToInt32(this.dataProtector.Unprotect(FromWebSafeBase64(code)), 0);
#endif

        /// <summary>
        /// Converts the specified <paramref name="data"/> to Base64 (web-safe).
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The base64 (web-safe).</returns>
        private static string ToWebSafeBase64(byte[] data)
             => Convert.ToBase64String(data, Base64FormattingOptions.None)
                       .Replace('+', '-')
                       .Replace('/', '_')
                       .TrimEnd('=');

        /// <summary>
        /// Converts the specified <paramref name="base64"/> (web-safe) to <c>byte[]</c>.
        /// </summary>
        /// <param name="base64">The base64.</param>
        /// <returns>The <c>byte[]</c> from the specified <paramref name="base64"/>.</returns>
        private static byte[] FromWebSafeBase64(string base64)
        {
            try
            {
                var s = new StringBuilder(base64)
                    .Replace('-', '+')
                    .Replace('_', '/');
                var padding = base64.Length % 4;
                if (padding != 0)
                {
                    s.Append('=', 4 - padding);
                }

                return Convert.FromBase64String(s.ToString());
            }
            catch (FormatException)
            {
                throw new CryptographicException();
            }
        }
    }
}