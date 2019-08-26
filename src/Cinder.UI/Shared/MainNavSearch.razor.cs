using System;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Clients;
using Cinder.UI.Infrastructure.Components;
using Cinder.UI.Infrastructure.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Cinder.UI.Shared
{
    public class MainNavSearchModel : EventBusComponent
    {
        public bool IsSearching { get; set; }

        public string Query { get; set; }

        [Inject]
        public IUriHelper UriHelper { get; set; }

        [Inject]
        public IApiClient ApiClient { get; set; }

        [Inject]
        public ILogger<MainNavSearchModel> Logger { get; set; }

        public async Task OnSearchClicked()
        {
            if (string.IsNullOrWhiteSpace(Query))
            {
                return;
            }

            if (Query.Length > 100)
            {
                return;
            }

            IsSearching = true;
            bool isError = false;
            SearchResultDto result = null;
            try
            {
                result = await ApiClient.Search(Query);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Search exception; Query: {Query}", Query);
                isError = true;
            }

            Query = null;
            IsSearching = false;

            if (isError || result == null)
            {
                return;
            }

            string url = "/search/error?";
            switch (result.Type)
            {
                case SearchResultType.BlockHash:
                    url = $"/block/{result.Id}";
                    break;
                case SearchResultType.BlockNumber:
                    url = $"/block-height/{result.Id}";
                    break;
                case SearchResultType.TransactionHash:
                    url = $"/transaction/{result.Id}";
                    break;
            }

            UriHelper.NavigateTo(url);
        }
    }
}
