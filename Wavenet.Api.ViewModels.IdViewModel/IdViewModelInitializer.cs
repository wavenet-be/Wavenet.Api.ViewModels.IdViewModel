// <copyright file="IdViewModelInitializer.cs" company="Wavenet">
// Copyright (c) Wavenet. All rights reserved.
// </copyright>

namespace Wavenet.Api.ViewModels
{
    using System;
    using System.Linq;

#if NETCOREAPP
    using Microsoft.Extensions.DependencyInjection;
#endif

    using Wavenet.Api.ViewModels.ProtectionProviders;

    /// <summary>
    /// <see cref="IdViewModelInitializer"/> initialize <see cref="IdViewModel"/>.
    /// </summary>
    public static class IdViewModelInitializer
    {
#if NETFRAMEWORK

        /// <summary>
        /// Configures <typeparamref name="TProtectionProvider"/> as <see cref="IdViewModel"/> protection engine.
        /// </summary>
        /// <typeparam name="TProtectionProvider">Type of <see cref="IIdViewModelProtectionProvider"/>.</typeparam>
        public static void Configure<TProtectionProvider>()
            where TProtectionProvider : IIdViewModelProtectionProvider, new()
#else
        /// <summary>
        /// Adds <typeparamref name="TProtectionProvider"/> as <see cref="IdViewModel"/> protection engine.
        /// </summary>
        /// <typeparam name="TProtectionProvider">Type of <see cref="IIdViewModelProtectionProvider"/>.</typeparam>
        /// <param name="services">The services.</param>
        public static void AddIdViewModel<TProtectionProvider>(this IServiceCollection services)
            where TProtectionProvider : class, IIdViewModelProtectionProvider
#endif
        {
            EnsureNotInitialized();
#if NETFRAMEWORK
            IdViewModel.ProtectionProvider = new Lazy<IIdViewModelProtectionProvider>(() => new TProtectionProvider());
#else
            var idViewModelProtectionProviderType = typeof(TProtectionProvider);
            if (!services.Any(s => s.ServiceType == idViewModelProtectionProviderType))
            {
                services.AddTransient(idViewModelProtectionProviderType);
            }

            IdViewModel.ProtectionProvider = new Lazy<IIdViewModelProtectionProvider>(() => services.BuildServiceProvider().GetService<TProtectionProvider>());
#endif
        }

#if NETFRAMEWORK

        /// <summary>
        /// Configures the specified <paramref name="protectionProvider"/> as <see cref="IdViewModel"/> protection engine.
        /// </summary>
        /// <param name="protectionProvider">The protection provider.</param>
        public static void Configure(IIdViewModelProtectionProvider protectionProvider)
            => Configure(() => protectionProvider);

#else
        /// <summary>
        /// Adds the specified <paramref name="protectionProvider"/> as <see cref="IdViewModel"/> protection engine.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="protectionProvider">The protection provider.</param>
        public static void AddIdViewModel(this IServiceCollection services, IIdViewModelProtectionProvider protectionProvider)
        {
            EnsureNotInitialized();
            IdViewModel.ProtectionProvider = new Lazy<IIdViewModelProtectionProvider>(protectionProvider);
        }
#endif

#if NETFRAMEWORK

        /// <summary>
        /// Configures the returned <see cref="IIdViewModelProtectionProvider"/> from <paramref name="protectionProvider"/> as <see cref="IdViewModel"/> protection engine.
        /// </summary>
        /// <param name="protectionProvider">The protection provider.</param>
        public static void Configure(Func<IIdViewModelProtectionProvider> protectionProvider)
#else
        /// <summary>
        /// Adds the returned <see cref="IIdViewModelProtectionProvider"/> from <paramref name="protectionProvider"/> as <see cref="IdViewModel"/> protection engine.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="protectionProvider">The protection provider.</param>
        public static void AddIdViewModel(this IServiceCollection services, Func<IIdViewModelProtectionProvider> protectionProvider)
#endif
        {
            EnsureNotInitialized();
            IdViewModel.ProtectionProvider = new Lazy<IIdViewModelProtectionProvider>(protectionProvider);
        }

        /// <summary>
        /// Ensures that <see cref="IdViewModel"/> is not already initialized.
        /// </summary>
        private static void EnsureNotInitialized()
        {
            if (IdViewModel.ProtectionProvider != null)
            {
                throw new InvalidOperationException("IdViewModel is already initialized");
            }
        }
    }
}