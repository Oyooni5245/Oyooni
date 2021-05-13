using Microsoft.Extensions.DependencyInjection;
using System;

namespace Oyooni.Server.Attributes
{
    /// <summary>
    /// Represents an attribute for injected services,
    /// Services with the <see cref="InjectedAttribute"/> get injected to the DI container automatically
    /// </summary>
    public class InjectedAttribute : Attribute
    {
        /// <summary>
        /// Gets the contract type to be registered
        /// </summary>
        public Type ContractType { get; }

        /// <summary>
        /// Gets the lifetime of the service
        /// </summary>
        public ServiceLifetime Lifetime { get; }

        /// <summary>
        /// Constructs an object of type <see cref="InjectedAttribute"/>
        /// </summary>
        /// <param name="lifetime">The lifetime of the service</param>
        /// <param name="contractType">The contract type to be registered, if is kept null then the service type is taken as the contract type</param>
        public InjectedAttribute(ServiceLifetime lifetime, Type contractType = null)
            => (ContractType, Lifetime) = (contractType ?? GetType(), lifetime);
    }
}
