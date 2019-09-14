using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cinder.Core.Exceptions;
using Cinder.Data.Repositories;
using Cinder.Documents;
using Microsoft.Extensions.Logging;
using Nethereum.Hex.HexTypes;
using Nethereum.Parity;
using Nethereum.Util;

namespace Cinder.Indexer.Infrastructure
{
    public class AddressRefresherRunner : IAddressRefresherRunner
    {
        private readonly IAddressRepository _addressRepository;
        private readonly ILogger<AddressRefresherRunner> _logger;
        private readonly IWeb3Parity _web3;

        public AddressRefresherRunner(ILogger<AddressRefresherRunner> logger, IAddressRepository addressRepository,
            IWeb3Parity web3)
        {
            _logger = logger;
            _addressRepository = addressRepository;
            _web3 = web3;
        }

        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                IEnumerable<CinderAddress> addresses = await _addressRepository
                    .GetStaleAddresses(cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                foreach (CinderAddress address in addresses)
                {
                    _logger.LogDebug("Updating balance for {Hash}", address.Hash);
                    HexBigInteger balance = await _web3.Eth.GetBalance.SendRequestAsync(address.Hash).ConfigureAwait(false);
                    address.Balance = UnitConversion.Convert.FromWei(balance);
                    address.CacheDate = DateTimeOffset.UtcNow;
                    address.ForceRefresh = false;
                    await _addressRepository.UpsertAddress(address, cancellationToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Class} -> {Method} -> Unexpected error", nameof(AddressRefresherRunner), nameof(RunAsync));
                throw new LoggedException(e);
            }
        }
    }
}
