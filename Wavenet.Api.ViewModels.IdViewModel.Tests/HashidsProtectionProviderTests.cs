// -----------------------------------------------------------------------
//  <copyright file="HashidsProtectionProviderTests.cs" company="Wavenet">
//  Copyright (c) Wavenet. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Wavenet.Api.ViewModels.IdViewModel.Tests
{
    using Wavenet.Api.ViewModels.ProtectionProviders;

    using Xunit;

    public class HashidsProtectionProviderTests
    {
        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-42)]
        [InlineData(0)]
        [InlineData(42)]
        [InlineData(int.MaxValue)]
        public void ProtectUnprotect(int id)
        {
            var provider = new HashidsProtectionProvider("ThisIsSaltz");

            var protectedId = provider.Protect(id);
            Assert.NotEqual(id.ToString(), protectedId);

            var unprotectedId = provider.Unprotect(protectedId);
            Assert.Equal(id, unprotectedId);
        }
    }
}