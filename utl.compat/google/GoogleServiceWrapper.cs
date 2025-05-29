using Google.Apis.Services;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace d9.utl.compat.google;
/// <summary>
/// A wrapper for a specific <see cref="BaseClientService">Google client service</see> created in a
/// specified <see cref="GoogleServiceContext"/> which contains helpful utilities for derived
/// classes to automate common functions, such as sending events to a particular Google calendar.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class GoogleServiceWrapper<T>
    where T : BaseClientService
{
    /// <summary>
    /// The wrapped <see cref="BaseClientService">Google client service</see> for this class.
    /// </summary>
    public T Service { get; protected set; }
    /// <summary>
    /// The <see cref="GoogleServiceContext"/> in which this wrapper was created, providing its
    /// authorization and logging to the wrapped <see cref="Service">Service</see>.
    /// </summary>
    public GoogleServiceContext Context { get; protected set; }
    /// <summary>
    /// A log to which to write any relevant events, in derived classes.
    /// </summary>
    public ILogger? Log => Context.Log;
    /// <summary>
    /// Creates a wrapper for the specified <paramref name="service"/> in the specified <paramref name="context"/>.
    /// </summary>
    /// <param name="service">The service to wrap.</param>
    /// <param name="context">The context which provides logging and authorization for the service.</param>
    /// <remarks>
    /// This is <see langword="protected"/> to allow overriding, but in general implementers should
    /// rely on <see cref="Create{W}(GoogleServiceContext, string[])">Create</see> and <see
    /// cref="TryCreate{W}(GoogleServiceContext, string[])">TryCreate</see>, as these methods ensure
    /// that the <paramref name="service"/> actually uses the relevant <paramref name="context"/>.
    /// </remarks>
    protected GoogleServiceWrapper(T service, GoogleServiceContext context)
    {
        Service = service;
        Context = context;
    }
    /// <summary>
    /// Creates a service wrapper for the specified <typeparamref name="T">service
    /// type</typeparamref> in the specified <paramref name="context"/> with the specified <paramref
    /// name="scopes"/>. <br/><br/><b>Throws an Exception</b> if creating the scope was not successful.
    /// </summary>
    /// <param name="context"><inheritdoc cref="TryCreate(GoogleServiceContext, string[])" path="/param[@name='context']"/></param>
    /// <param name="scopes"><inheritdoc cref="TryCreate(GoogleServiceContext, string[])" path="/param[@name='scopes']"/></param>
    /// <returns>A wrapper for the specified service.</returns>
    /// <exception cref="Exception">Thrown if the wrapper was not able to be created.</exception>
    public static W Create<W>(GoogleServiceContext context, params string[] scopes)
        where W : GoogleServiceWrapper<T>
    {
        W? wrapper = TryCreate<W>(context, scopes);
        if (wrapper is W result)
            return result;
        throw new Exception($"Could not initialize GoogleServiceWrapper<{typeof(T).Name}> with context {context} and {"scope".Plural(scopes)} {scopes.ListNotation()}!");
    }
    /// <summary>
    /// Tries to create a service wrapper for the specified <typeparamref name="T">service
    /// type</typeparamref> in the specified <paramref name="context"/> with the specified <paramref name="scopes"/>.
    /// </summary>
    /// <param name="context">
    /// The <see cref="GoogleServiceContext"/> which provides authentication and logging for the result.
    /// </param>
    /// <param name="scopes">The authorization scopes assigned to the result.</param>
    /// <returns>
    /// A wrapper for the specified service, if successful, or <see langword="null"/> otherwise.
    /// </returns>
    public static W? TryCreate<W>(GoogleServiceContext context, params string[] scopes)
        where W : GoogleServiceWrapper<T>
    {
        if (Activator.CreateInstance(typeof(T), context.InitializerFor(scopes)) is T service)
            return Activator.CreateInstance(typeof(W), bindingAttr: BindingFlags.NonPublic | BindingFlags.Instance, null, [service, context], null) as W;
        return null;
    }
}