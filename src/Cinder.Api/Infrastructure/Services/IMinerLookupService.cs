namespace Cinder.Api.Infrastructure.Services
{
    public interface IMinerLookupService
    {
        string GetByAddressOrDefault(string hash);
    }
}
