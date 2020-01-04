// <copyright file="DevelopmentNoProtectionProvider.cs" company="Wavenet">
// Copyright (c) Wavenet. All rights reserved.
// </copyright>

namespace Wavenet.Api.ViewModels.ProtectionProviders
{
    using System;
    using System.Globalization;
    using System.Security.Cryptography;
#if NETFRAMEWORK
    using System.Web;
#else
    using Microsoft.AspNetCore.Hosting;
#endif

    /// <summary>
    /// <see cref="DevelopmentNoProtectionProvider"/> simplify debug use of <see cref="IdViewModel"/> by sending code as id.
    /// </summary>
    /// <seealso cref="IIdViewModelProtectionProvider" />
    public class DevelopmentNoProtectionProvider : IIdViewModelProtectionProvider
    {
#if NETFRAMEWORK
        /// <summary>
        /// Initializes a new instance of the <see cref="DevelopmentNoProtectionProvider" /> class.
        /// </summary>
        public DevelopmentNoProtectionProvider()
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="DevelopmentNoProtectionProvider" /> class.
        /// </summary>
        /// <param name="hostingEnvironment">The hosting environment.</param>
        public DevelopmentNoProtectionProvider(IHostingEnvironment hostingEnvironment)
#endif
        {
#if NETFRAMEWORK
            if (!HttpContext.Current.IsDebuggingEnabled)
#else
            if (!hostingEnvironment.IsDevelopment())
#endif
            {
                throw new InvalidOperationException("Avoid use of DebugNoProtectionProvider in production environment.");
            }
        }

        /// <inheritdoc />
        public string Protect(int id)
            => id.ToString(CultureInfo.InvariantCulture);

        /// <inheritdoc />
        public int Unprotect(string code)
            => int.TryParse(code, out var id) ? id : throw new CryptographicException();
    }
}