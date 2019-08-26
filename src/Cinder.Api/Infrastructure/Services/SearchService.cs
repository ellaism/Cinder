using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Cinder.Api.Infrastructure.Models;
using Cinder.Api.Infrastructure.Repositories;

namespace Cinder.Api.Infrastructure.Services
{
    public class SearchService : ISearchService
    {
        private readonly IAddressReadOnlyRepository _addressRepository;
        private readonly IBlockReadOnlyRepository _blockRepository;
        private readonly ITransactionReadOnlyRepository _transactionRepository;

        public SearchService(IAddressReadOnlyRepository addressRepository, IBlockReadOnlyRepository blockRepository,
            ITransactionReadOnlyRepository transactionRepository)
        {
            _addressRepository = addressRepository;
            _blockRepository = blockRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<SearchResult> ExecuteSearch(string input)
        {
            string query = Regex.Replace(input ?? "", @"\s+", "");
            SearchResult searchResult = new SearchResult();

            if (Regex.IsMatch(query, "^(0x)?[0-9a-f]{40}$", RegexOptions.IgnoreCase))
            {
                query = query.StartsWith("0x") ? query : $"0x{query}";

                // todo
                searchResult.Id = query;
                searchResult.Type = SearchResultType.AddressHash;
            }
            else if (Regex.IsMatch(query, "^(0x)?[0-9a-f]{64}$", RegexOptions.IgnoreCase))
            {
                query = query.StartsWith("0x") ? query : $"0x{query}";

                string result = await _blockRepository.GetBlockHashIfExists(query).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(result))
                {
                    searchResult.Id = result;
                    searchResult.Type = SearchResultType.BlockHash;

                    return searchResult;
                }

                result = await _transactionRepository.GetTransactionHashIfExists(query).ConfigureAwait(false);
                if (string.IsNullOrEmpty(result))
                {
                    return searchResult;
                }

                searchResult.Id = result;
                searchResult.Type = SearchResultType.TransactionHash;
            }
            else if (ulong.TryParse(query, out ulong blockNumber))
            {
                string result = await _blockRepository.GetBlockNumberIfExists(blockNumber).ConfigureAwait(false);
                if (string.IsNullOrEmpty(result))
                {
                    return searchResult;
                }

                searchResult.Id = result;
                searchResult.Type = SearchResultType.BlockNumber;
            }

            return searchResult;
        }
    }
}
