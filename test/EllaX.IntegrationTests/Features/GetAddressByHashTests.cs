using System.Threading.Tasks;
using EllaX.Application.Features.Address;
using Shouldly;
using Xunit;

namespace EllaX.IntegrationTests.Features
{
    using static SliceFixture;

    public class GetAddressByHashTests
    {
        [Fact]
        public async Task Should_get_address_by_hash()
        {
            GetAddressByHash.Model result =
                await SendAsync(new GetAddressByHash.Query {Hash = "0xe9c2d958e6234c862b4afbd75b2fd241e9556303"});

            result.ShouldNotBeNull();
        }
    }
}
