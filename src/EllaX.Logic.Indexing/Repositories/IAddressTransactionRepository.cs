﻿using System.Threading.Tasks;
using EllaX.Core.Models;
using EllaX.Data;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Transaction = Nethereum.RPC.Eth.DTOs.Transaction;

namespace EllaX.Logic.Indexing.Repositories
{
    public interface IAddressTransactionRepository
    {
        Task UpsertAsync(Transaction transaction, TransactionReceipt transactionReceipt, bool failedCreatingContract,
            HexBigInteger blockTimestamp, string address, string error = null, bool hasVmStack = false,
            string newContractAddress = null);

        Task<ITransactionView> FindByAddressBlockNumberAndHashAsync(string address, HexBigInteger blockNumber,
            string transactionHash);
    }
}
