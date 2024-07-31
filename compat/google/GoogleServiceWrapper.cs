using Google.Apis.Services;

namespace d9.utl.compat.google;
public class GoogleServiceWrapper<T>
    where T : BaseClientService
{
    public T Service { get; protected set; }
    protected GoogleServiceWrapper(T service)
    {
        Service = service;
    }
}
