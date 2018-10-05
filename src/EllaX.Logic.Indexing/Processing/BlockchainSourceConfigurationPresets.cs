using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace EllaX.Logic.Indexing.Processing
{
    public static class BlockchainSourceConfigurationPresets
    {
        public static Dictionary<string, BlockchainSourceConfiguration> All =
            new Dictionary<string, BlockchainSourceConfiguration>
            {
                {
                    "localhost", new BlockchainSourceConfiguration("http://localhost:8545", "localhost") {FromBlock = 0}
                },
                {
                    "rinkeby",
                    new BlockchainSourceConfiguration("https://rinkeby.infura.io/v3/25e7b6dfc51040b3bfc0e47317d38f60",
                        "rinkeby") {MinimumBlockNumber = 2830143}
                }
            };

        public static string Default { get; } = "localhost";

        public static BlockchainSourceConfiguration Get(IConfigurationRoot config)
        {
            string presetName = config["Blockchain"] ?? Default;
            BlockchainSourceConfiguration configuration = Get(presetName);

            ApplyOverrides(config, configuration);

            return configuration;
        }

        private static void ApplyOverrides(IConfigurationRoot config, BlockchainSourceConfiguration configuration)
        {
            long? minBlockNumber = Parse(config, "MinimumBlockNumber");
            long? fromBlock = Parse(config, "FromBlock");
            long? toBlock = Parse(config, "ToBlock");
            string blockchainUrl = config["BlockchainUrl"];
            string blockchainName = config["Blockchain"];
            bool? postVm = ParseBool(config, "PostVm");
            bool? processTransactionsInParallel = ParseBool(config, "ProcessBlockTransactionsInParallel");

            if (minBlockNumber != null)
            {
                configuration.MinimumBlockNumber = minBlockNumber;
            }

            if (fromBlock != null)
            {
                configuration.FromBlock = fromBlock;
            }

            if (toBlock != null)
            {
                configuration.ToBlock = toBlock;
            }

            if (blockchainUrl != null)
            {
                configuration.BlockchainUrl = blockchainUrl;
            }

            if (blockchainName != null)
            {
                configuration.Name = blockchainName;
            }

            if (postVm != null)
            {
                configuration.PostVm = postVm.Value;
            }

            if (processTransactionsInParallel != null)
            {
                configuration.ProcessBlockTransactionsInParallel = processTransactionsInParallel.Value;
            }
        }

        public static long? Parse(IConfigurationRoot config, string name)
        {
            string configVal = config[name];

            return string.IsNullOrEmpty(configVal) ? (long?) null : long.Parse(configVal);
        }

        public static bool? ParseBool(IConfigurationRoot config, string name)
        {
            string configVal = config[name];

            return string.IsNullOrEmpty(configVal) ? (bool?) null : bool.Parse(configVal);
        }

        public static BlockchainSourceConfiguration Get(string name)
        {
            if (!All.ContainsKey(name))
            {
                throw new Exception($"There is no preset configuration for '{name}'");
            }

            return All[name];
        }
    }
}
