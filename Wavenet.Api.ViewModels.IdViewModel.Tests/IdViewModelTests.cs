// -----------------------------------------------------------------------
//  <copyright file="IdViewModelTests.cs" company="Wavenet">
//  Copyright (c) Wavenet. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Wavenet.Api.ViewModels.IdViewModelTests
{
    using System;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;

    using Xunit;

    public class IdViewModelTests : IDisposable
    {
        private readonly TestServer server;

        public IdViewModelTests()
        {
            this.server = new TestServer(new WebHostBuilder().UseStartup<TestStartup>().UseEnvironment(EnvironmentName.Development));
        }

        public void Dispose()
        {
            this.server.Dispose();
        }

        [Fact]
        public void IdViewModelEquals()
        {
            IdViewModel firstId = 1,
                        secondId = 2,
                        thirdId = 1,
                        fourthId = null;
            object nonIdViewModel = new object();

            Assert.Equal(firstId, thirdId);
            Assert.True(firstId.Equals((object)thirdId), "firstId should be equal to thirdId.");
            Assert.NotEqual(firstId, secondId);
            Assert.NotEqual(firstId, fourthId);
            Assert.False(firstId.Equals((object)fourthId), "firstId should be different from fourthId.");
            Assert.False(firstId.Equals(nonIdViewModel), "firstId should be different from nonIdViewModel.");
        }
    }
}