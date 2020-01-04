// <copyright file="HashidsProtectionProvider.cs" company="Wavenet">
// Copyright (c) Wavenet. All rights reserved.
// </copyright>

namespace Wavenet.Api.ViewModels.ProtectionProviders
{
    using System.Security.Cryptography;

    using HashidsNet;

    /// <summary>
    /// <see cref="HashidsProtectionProvider"/> uses <see cref="Hashids"/> as protection mechanism for <see cref="IdViewModel"/>.
    /// </summary>
    /// <seealso cref="IIdViewModelProtectionProvider" />
    public class HashidsProtectionProvider : IIdViewModelProtectionProvider
    {
        /// <summary>
        /// The default alphabet.
        /// </summary>
        public const string DefaultAlphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        /// <summary>
        /// The default seps.
        /// </summary>
        public const string DefaultSeps = "cfhistuCFHISTU";

        /// <summary>
        /// The minimum hash length.
        /// </summary>
        public const int MinHashLength = 0;

        /// <summary>
        /// The protection.
        /// </summary>
        private readonly Hashids protection;

        /// <summary>
        /// Initializes a new instance of the <see cref="HashidsProtectionProvider" /> class.
        /// </summary>
        /// <param name="salt">The salt.</param>
        /// <param name="minHashLength">Minimum length of the hash.</param>
        /// <param name="alphabet">The alphabet.</param>
        /// <param name="seps">The seps.</param>
        public HashidsProtectionProvider(string salt, int minHashLength = MinHashLength, string alphabet = DefaultAlphabet, string seps = DefaultSeps)
        {
            this.protection = new Hashids(salt, minHashLength, alphabet, seps);
        }

        /// <inheritdoc />
        public string Protect(int id)
            => this.protection.Encode(id);

        /// <inheritdoc />
        public int Unprotect(string code)
        {
            var ids = this.protection.Decode(code);
            return ids.Length == 1 ? ids[0] : throw new CryptographicException();
        }
    }
}