using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EllaX.Logic.Indexing.Exceptions;
using EllaX.Logic.Indexing.Processing.PostProcessors;
using EllaX.Logic.Indexing.Repositories;
using EllaX.Logic.Indexing.Transactions;
using Nethereum.Web3;

namespace EllaX.Logic.Indexing.Processing
{
    public class StorageProcessor : IDisposable
    {
        private const int MaxRetries = 3;
        private readonly IBlockRepository _blockRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IBlockProcessor _processor;
        private readonly List<object> _repositories = new List<object>();
        private readonly WaitForBlockStrategy _waitForBlockStrategy;
        private readonly Web3 _web3;
        private bool _contractCacheInitialised;

        public StorageProcessor(string url, IBlockchainStoreRepositoryFactory repositoryFactory, bool postVm = false)
        {
            _waitForBlockStrategy = new WaitForBlockStrategy();
            _web3 = new Web3(url);

            _blockRepository = repositoryFactory.CreateBlockRepository();
            _contractRepository = repositoryFactory.CreateContractRepository();

            ITransactionRepository transactionRepository = repositoryFactory.CreateTransactionRepository();
            IAddressTransactionRepository addressTransactionRepository =
                repositoryFactory.CreateAddressTransactionRepository();
            ITransactionLogRepository logRepository = repositoryFactory.CreateTransactionLogRepository();
            ITransactionVmStackRepository vmStackRepository = repositoryFactory.CreateTransactionVmStackRepository();

            _repositories.Add(_blockRepository);
            _repositories.Add(transactionRepository);
            _repositories.Add(addressTransactionRepository);
            _repositories.Add(_contractRepository);
            _repositories.Add(logRepository);
            _repositories.Add(vmStackRepository);

            ContractTransactionProcessor contractTransactionProcessor = new ContractTransactionProcessor(_web3,
                _contractRepository, transactionRepository, addressTransactionRepository, vmStackRepository,
                logRepository);
            ContractCreationTransactionProcessor contractCreationTransactionProcessor =
                new ContractCreationTransactionProcessor(_web3, _contractRepository, transactionRepository,
                    addressTransactionRepository);
            ValueTransactionProcessor valueTrasactionProcessor = new ValueTransactionProcessor(transactionRepository,
                addressTransactionRepository);
            TransactionProcessor transactionProcessor = new TransactionProcessor(_web3, contractTransactionProcessor,
                valueTrasactionProcessor, contractCreationTransactionProcessor);

            if (postVm)
            {
                _processor = new BlockVmPostProcessor(_web3, _blockRepository, transactionProcessor);
            }
            else
            {
                transactionProcessor.ContractTransactionProcessor.EnabledVmProcessing = false;
                _processor = new BlockProcessor(_web3, _blockRepository, transactionProcessor);
            }
        }

        public bool ProcessTransactionsInParallel
        {
            get => BlockProcessor.ProcessTransactionsInParallel;
            set => BlockProcessor.ProcessTransactionsInParallel = value;
        }

        public long? MinimumBlockNumber { get; set; }

        private async Task InitContractCache()
        {
            if (!_contractCacheInitialised)
            {
                await _contractRepository.FillCache().ConfigureAwait(false);
                _contractCacheInitialised = true;
            }
        }

        /// <summary>
        ///     Allow the processor to resume from where it left off
        /// </summary>
        private async Task<long> GetStartingBlockNumber()
        {
            long blockNumber = await _blockRepository.GetMaxBlockNumber();
            blockNumber = blockNumber <= 0 ? 0 : blockNumber - 1;

            if (MinimumBlockNumber.HasValue && MinimumBlockNumber > blockNumber)
            {
                return MinimumBlockNumber.Value;
            }

            return blockNumber;
        }

        public async Task<bool> ExecuteAsync(long? startBlock, long? endBlock, int retryNumber = 0)
        {
            startBlock = startBlock ?? await GetStartingBlockNumber();
            endBlock = endBlock ?? long.MaxValue;
            bool runContinuously = endBlock == long.MaxValue;

            await InitContractCache();

            while (startBlock <= endBlock)
            {
                try
                {
                    Console.WriteLine($"{DateTime.Now.ToString("s")}. Block: {startBlock}. Attempt: {retryNumber}");

                    await _processor.ProcessBlockAsync(startBlock.Value).ConfigureAwait(false);
                    retryNumber = 0;
                    startBlock = startBlock + 1;
                }
                catch (BlockNotFoundException blockNotFoundException)
                {
                    Console.WriteLine(blockNotFoundException.Message);

                    if (runContinuously)
                    {
                        Console.WriteLine("Waiting for block...");
                        await _waitForBlockStrategy.Apply(retryNumber);
                        await ExecuteAsync(startBlock, endBlock, retryNumber + 1);
                    }
                    else
                    {
                        if (retryNumber != MaxRetries)
                        {
                            await ExecuteAsync(startBlock, endBlock, retryNumber + 1).ConfigureAwait(false);
                        }
                        else
                        {
                            retryNumber = 0;
                            startBlock = startBlock + 1;
                            //Log.Error().Exception(blockNotFoundException).Message("BlockNumber" + startBlock).Write();
                            Console.WriteLine("Skipping block");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ". " + ex.InnerException?.Message);

                    if (retryNumber != MaxRetries)
                    {
                        await ExecuteAsync(startBlock, endBlock, retryNumber + 1).ConfigureAwait(false);
                    }
                    else
                    {
                        retryNumber = 0;
                        startBlock = startBlock + 1;
                        //Log.Error().Exception(ex).Message("BlockNumber" + startBlock).Write();
                        Console.WriteLine("ERROR:" + startBlock + " " + DateTime.Now.ToString("s"));
                    }
                }
            }

            return true;
        }

        public class WaitForBlockStrategy
        {
            private readonly int[] waitIntervals = {1000, 2000, 5000, 10000, 15000};

            public async Task Apply(int retryCount)
            {
                int intervalMs = retryCount >= waitIntervals.Length ? waitIntervals.Last() : waitIntervals[retryCount];
                await Task.Delay(intervalMs);
            }
        }

        #region IDisposable Support

        private bool disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (object repo in _repositories)
                    {
                        if (repo is IDisposable disposableRepo)
                        {
                            disposableRepo.Dispose();
                        }
                    }

                    _repositories.Clear();
                }

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~StorageProcessor() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
