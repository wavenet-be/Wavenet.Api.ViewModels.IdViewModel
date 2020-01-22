// <copyright file="IdViewModel.cs" company="Wavenet">
// Copyright (c) Wavenet. All rights reserved.
// </copyright>

namespace Wavenet.Api.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Security.Cryptography;

    using Wavenet.Api.ViewModels.ProtectionProviders;

    /// <summary>
    /// A dedicated ViewModel to expose internal IDs outside of the application.
    /// </summary>
    /// <seealso cref="IEquatable{IdViewModel}" />
    [TypeConverter(typeof(Converter))]
    [DebuggerDisplay("{" + nameof(Id) + "}")]
    public class IdViewModel : IEquatable<IdViewModel>, IValidatableObject
    {
        /// <summary>
        /// The identifier.
        /// </summary>
        private readonly int id;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdViewModel"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public IdViewModel(int id)
        {
            this.id = id;
            this.Code = EnsureProtectionProvider().Protect(id);
            this.IsValid = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdViewModel"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public IdViewModel(string code)
        {
            try
            {
                this.id = EnsureProtectionProvider().Unprotect(code);
                this.Code = code;
                this.IsValid = true;
            }
            catch (CryptographicException)
            {
                this.IsValid = false;
            }
        }

        /// <summary>
        /// Gets the protection provider.
        /// </summary>
        /// <value>
        /// The protection provider.
        /// </value>
        public static Lazy<IIdViewModelProtectionProvider> ProtectionProvider { get; internal set; }

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id => this.IsValid ? this.id : throw new CryptographicException("Invalid identifier");

        /// <summary>
        /// Gets a value indicating whether this <c>Id</c> is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this <c>Id</c> is valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid { get; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="int"/> to <see cref="IdViewModel"/>.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator IdViewModel(int id) => new IdViewModel(id);

        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="IdViewModel"/>.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator IdViewModel(string code) => new IdViewModel(code);

        /// <summary>
        /// Performs an implicit conversion from <see cref="IdViewModel"/> to <see cref="int"/>.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator int(IdViewModel model) => model.Id;

        /// <summary>
        /// Performs an implicit conversion from <see cref="IdViewModel"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(IdViewModel model) => model.Code;

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(IdViewModel x, IdViewModel y) => !(x == y);

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(IdViewModel x, IdViewModel y) => ReferenceEquals(x, y) || x?.Equals(y) == true;

        /// <inheritdoc />
        public bool Equals(IdViewModel other) => this.Id == other?.Id;

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is IdViewModel id && this.Equals(id);

        /// <inheritdoc />
        public override int GetHashCode() => this.Id.GetHashCode();

        /// <inheritdoc />
        public override string ToString() => this.Code;

        /// <summary>
        /// Determines whether the specified object is valid.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>
        /// A collection that holds failed-validation information.
        /// </returns>
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (!this.IsValid)
            {
                return new[] { new ValidationResult("Invalid identifier") };
            }
            else
            {
                return Enumerable.Empty<ValidationResult>();
            }
        }

        /// <summary>
        /// Ensures the protection provider is correctly initialized.
        /// </summary>
        /// <returns>The underlying <see cref="IIdViewModelProtectionProvider"/>.</returns>
        private static IIdViewModelProtectionProvider EnsureProtectionProvider()
            => ProtectionProvider?.Value ?? throw new InvalidOperationException("IdViewModel is not initialized!");

        /// <summary>
        /// <see cref="TypeConverter"/> for <see cref="IdViewModel"/>.
        /// </summary>
        /// <seealso cref="TypeConverter" />
        private class Converter : TypeConverter
        {
            /// <summary>
            /// The valid types.
            /// </summary>
            private static readonly HashSet<Type> ValidTypes = new HashSet<Type> { typeof(string), typeof(int), typeof(IdViewModel) };

            /// <summary>
            /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
            /// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type you want to convert from.</param>
            /// <returns>
            ///   <see langword="true" /> if this converter can perform the conversion; otherwise, <see langword="false" />.
            /// </returns>
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => ValidTypes.Contains(sourceType);

            /// <summary>
            /// Returns whether this converter can convert the object to the specified type, using the specified context.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
            /// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type you want to convert to.</param>
            /// <returns>
            ///   <see langword="true" /> if this converter can perform the conversion; otherwise, <see langword="false" />.
            /// </returns>
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) => destinationType == typeof(string);

            /// <summary>
            /// Converts the given object to the type of this converter, using the specified context and culture information.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
            /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture.</param>
            /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
            /// <returns>
            /// An <see cref="T:System.Object" /> that represents the converted value.
            /// </returns>
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                switch (value)
                {
                    case string code:
                        return new IdViewModel(code);

                    case int id:
                        return new IdViewModel(id);

                    case IdViewModel id:
                        return id;

                    default:
                        throw new NotSupportedException();
                }
            }

            /// <summary>
            /// Converts the given value object to the specified type, using the specified context and culture information.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
            /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" />. If <see langword="null" /> is passed, the current culture is assumed.</param>
            /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
            /// <param name="destinationType">The <see cref="T:System.Type" /> to convert the <paramref name="value" /> parameter to.</param>
            /// <returns>
            /// An <see cref="T:System.Object" /> that represents the converted value.
            /// </returns>
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (value is IdViewModel data)
                {
                    return data.Code;
                }

                return null;
            }
        }
    }
}