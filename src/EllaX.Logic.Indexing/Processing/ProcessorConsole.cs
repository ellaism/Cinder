using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EllaX.Logic.Indexing.Repositories;

namespace EllaX.Logic.Indexing.Processing
{
    public class ProcessorConsole
    {
        private static StorageProcessor _proc;

        public static async Task<int> Execute(IBlockchainStoreRepositoryFactory repositoryFactory,
            BlockchainSourceConfiguration configuration)
        {
            using (_proc = new StorageProcessor(configuration.BlockchainUrl, repositoryFactory, configuration.PostVm)
            {
                MinimumBlockNumber = configuration.MinimumBlockNumber,
                ProcessTransactionsInParallel = configuration.ProcessBlockTransactionsInParallel
            })
            {
                //this should not really be necessary
                //but without it, when the process is killed early, some csv records where not being flushed
                AppDomain.CurrentDomain.ProcessExit += (s, e) => { _proc?.Dispose(); };

                Stopwatch stopWatch = Stopwatch.StartNew();

                bool result = await _proc.ExecuteAsync(configuration.FromBlock, configuration.ToBlock)
                    .ConfigureAwait(false);

                Console.WriteLine("Duration: " + stopWatch.Elapsed);

                Debug.WriteLine($"Finished With Success: {result}");
                Console.WriteLine("Finished. Success:" + result);
                Console.ReadLine();

                return result ? 0 : 1;
            }
        }
    }
}
