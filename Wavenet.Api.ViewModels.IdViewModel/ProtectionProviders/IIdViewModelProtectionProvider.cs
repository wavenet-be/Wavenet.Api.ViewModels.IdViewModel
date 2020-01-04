// <copyright file="IIdViewModelProtectionProvider.cs" company="Wavenet">
// Copyright (c) Wavenet. All rights reserved.
// </copyright>

namespace Wavenet.Api.ViewModels.ProtectionProviders
{
    /// <summary>
    /// <see cref="IIdViewModelProtectionProvider"/> is the protection engine used by <see cref="IdViewModel"/>.
    /// </summary>
    public interface IIdViewModelProtectionProvider
    {
        /// <summary>
        /// Protects the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The protected id.
        /// </returns>
        string Protect(int id);

        /// <summary>
        /// Unprotects the specified code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>
        /// The underlying id.
        /// </returns>
        int Unprotect(string code);
    }
}
