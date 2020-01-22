// -----------------------------------------------------------------------
//  <copyright file="DevelopmentNoProtectionProviderTests.cs" company="Wavenet">
//  Copyright (c) Wavenet. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Wavenet.Api.ViewModels.IdViewModelTests
{
    using System;

    using Microsoft.AspNetCore.Hosting.Internal;

    using Wavenet.Api.ViewModels.ProtectionProviders;

    using Xunit;

    public class DevelopmentNoProtectionProviderTests
    {
        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-42)]
        [InlineData(0)]
        [InlineData(42)]
        [InlineData(int.MaxValue)]
        public void ProtectUnprotect(int id)
        {
            var fakeEnvironment = new HostingEnvironment();
            fakeEnvironment.EnvironmentName = "Development";
            var provider = new DevelopmentNoProtectionProvider(fakeEnvironment);

            var protectedId = provider.Protect(id);
            Assert.Equal(id.ToString(), protectedId);

            var unprotectedId = provider.Unprotect(protectedId);
            Assert.Equal(id, unprotectedId);
        }

        [Fact]
        public void CannotBeUsedInProduction()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var fakeEnvironment = new HostingEnvironment();
                fakeEnvironment.EnvironmentName = "Production";
                var provider = new DevelopmentNoProtectionProvider(fakeEnvironment);

                provider.Protect(42);
            });
        }
    }
}