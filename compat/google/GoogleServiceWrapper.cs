using Google.Apis.Services;

namespace d9.utl.compat.google;
public class GoogleServiceWrapper<T>
    where T : BaseClientService
{
    public T Service { get; protected set; }
    public Log? Log { get; protected set; }
    public GoogleServiceWrapper(T service, Log? log = null)
    {
        Service = service;
        Log = log;
    }
    public static GoogleServiceWrapper<T> CreateFrom(GoogleServiceContext context, params string[] scopes)
    {
        GoogleServiceWrapper<T>? wrapper = TryCreateFrom(context, scopes);
        if (wrapper is GoogleServiceWrapper<T> result)
            return result;
        throw new Exception($"Could not initialize GoogleServiceWrapper<{typeof(T).Name}> with context {context} and {"scope".Plural(scopes)} {scopes.ListNotation()}!");
    }
    public static GoogleServiceWrapper<T>? TryCreateFrom(GoogleServiceContext context, params string[] scopes)
    {
        if (Activator.CreateInstance(typeof(T), context.InitializerFor(scopes)) is T service)
            return new(service, context.Log);
        return null;
    }
}
