// -----------------------------------------------------------------------
//  <copyright file="MachineKeyProtectionProviderTests.cs" company="Wavenet">
//  Copyright (c) Wavenet. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Wavenet.Api.ViewModels.IdViewModel.Tests
{
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Hosting.Internal;

    using Wavenet.Api.ViewModels.ProtectionProviders;

    using Xunit;

    public class MachineKeyProtectionProviderTests
    {
        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-42)]
        [InlineData(0)]
        [InlineData(42)]
        [InlineData(int.MaxValue)]
        public void ProtectUnprotect(int id)
        {
            var dataProtectionProvider = DataProtectionProvider.Create("Test App");
            var provider = new MachineKeyProtectionProvider(dataProtectionProvider);

            var protectedId = provider.Protect(id);
            Assert.NotEqual(id.ToString(), protectedId);

            var unprotectedId = provider.Unprotect(protectedId);
            Assert.Equal(id, unprotectedId);
        }
    }
}