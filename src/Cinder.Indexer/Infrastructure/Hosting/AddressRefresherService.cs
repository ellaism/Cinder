using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cinder.Core.Exceptions;
using Cinder.Data.Repositories;
using Cinder.Documents;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nethereum.Hex.HexTypes;
using Nethereum.Parity;
using Nethereum.Util;

namespace Cinder.Indexer.Infrastructure.Hosting
{
    public class AddressRefresherService : BackgroundService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly ILogger<AddressRefresherService> _logger;
        private readonly IWeb3Parity _web3;

        public AddressRefresherService(ILogger<AddressRefresherService> logger, IAddressRepository addressRepository,
            IWeb3Parity web3)
        {
            _logger = logger;
            _addressRepository = addressRepository;
            _web3 = web3;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    IEnumerable<CinderAddress> addresses = await _addressRepository.GetStaleAddresses(stoppingToken);

                    foreach (CinderAddress address in addresses)
                    {
                        _logger.LogDebug("Updating balance for {Hash}", address.Hash);
                        HexBigInteger balance = await _web3.Eth.GetBalance.SendRequestAsync(address.Hash);
                        address.Balance = UnitConversion.Convert.FromWei(balance);
                        address.CacheDate = DateTimeOffset.UtcNow;
                        await _addressRepository.UpsertAddress(address, stoppingToken);
                    }
                }
                catch (LoggedException) { }
                catch (Exception e)
                {
                    _logger.LogError(e, "Address refresher threw a non-logged exception");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
