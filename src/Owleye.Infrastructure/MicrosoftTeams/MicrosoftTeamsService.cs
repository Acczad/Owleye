using System.Threading.Tasks;
using System;
using Owleye.Shared.Cache;
using Extension.Methods;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Owleye.Shared.Model.MicrosoftTeams;
using Owleye.Shared.MicrosoftTeams;

namespace Owleye.Infrastructure.MicrosoftTeams
{
    public class MicrosoftTeamsService : IMicrosoftTeamsService
    {
        private readonly IRedisCache _redisCache;
        private readonly IConfiguration _configuration;
        private string _baseUrl;
        public MicrosoftTeamsService(
            IRedisCache redisCache,
            IConfiguration configuration)
        {
            _redisCache=redisCache;
            _configuration=configuration;
            _baseUrl = configuration["MicrosoftGraphApi.BaseUrl"];
        }

        public async Task SendTeamsMessageAsync(SendTeamsUsersMessageRequest request)
        {
            var token = await CheckTokenValidityAsync();
            var me = await GetMeAsync();
            foreach (var mail in request.Emails)
            {
                var chatResponseId = await CreateTeamsChatAsync(request, null, me.Id);
                if (chatResponseId.IsNotNullOrEmpty())
                {
                    var url = GetFullUrl($"/chats/{chatResponseId}/messages");
                    var message = new GraphSendMessageInChat(request.Message);
                    var chatResult = PostAsJsonAsync<string>(message, url, token);
                }
            }
        }
        private async Task<string> GetTeamsUserIdByEmailAsync(string email)
        {
            var token = await CheckAndGetToken();
            if (token.IsNullOrEmpty()) throw new Exception("MicrosoftTeams User Not logged-in");

            var url = GetFullUrl($"/users?$filter=startsWith(mail,'{email.ToLower()}')");

            var users = await GetAsync<GraphUserList>(url, token);

            if (users.Value?.Count == 0)
            {
                if (email.IndexOf('@') > 0)
                {
                    email = email.Remove(email.IndexOf('@'));
                    return await GetTeamsUserIdByEmailAsync(email);
                }
                return null;
            }

            return users.Value[0].Id;

        }
        private async Task<string> CreateTeamsChatAsync(SendTeamsUsersMessageRequest request, string topic, string senderUserId = "")
        {
            var token = await CheckAndGetToken();

            string url = GetFullUrl("/chats");

            var chatType = request.Emails.Count > 1 ? "group" : "oneOnOne";

            var userIds = new List<string>();
            if (!string.IsNullOrEmpty(senderUserId))
                userIds.Add(senderUserId);

            foreach (var email in request.Emails)
            {
                var userIdOfEmail = await GetTeamsUserIdByEmailAsync(email);
                if (userIdOfEmail != null)
                    userIds.Add(userIdOfEmail);
            }

            // Chats Must be between at least 2 users
            if (userIds.Count < 2)
                return null;


            var newChatRequstData = new MicrosoftTeamsConversation(chatType, topic, _baseUrl, userIds);
            var newChatResultData = await CallHttpAsJsonCall<GraphCreateChatResult>(newChatRequstData,url, token);
            if (newChatResultData.Id.IsNotNullOrEmpty())
            {
                return newChatResultData.Id;
            }

            return null;
        }
        private async Task<string> CheckTokenValidityAsync()
        {
            var token = await CheckAndGetTokenAsync();
            if (token.IsNullOrEmpty()) throw new Exception("MicrosoftTeams User Not logged-in");
            return token;
        }
        private async Task<MicrosoftTemasUserModel> GetMeAsync()
        {
            var cachedMe = await GetMeFromCacheAsync();
            if (cachedMe != null) return cachedMe;

            var token = await GetCurrentToken();
            if (token.IsNullOrEmpty()) return null;

            var url = GetFullUrl($"/me");
            var me = await GetAsync<MicrosoftTemasUserModel>(url,token);

            if (me == null || string.IsNullOrEmpty(me.Id))
                return null;

            await SaveMeCacheAsync(me);

            return me;
        }
        private string GetFullUrl(string path)
        {
            return _baseUrl + path;
        }
        private async Task<string> GetCurrentTokenAsync()
        {
            var token = await _redisCache.GetAsync<string>(_configuration["MicrosoftGraphApi.CacheKeyForToken"]);
            return token;
        }
        private async Task<string> CheckAndGetTokenAsync()
        {
            var token = await _redisCache.GetAsync<string>(_configuration["MicrosoftGraphApi:CacheKeyForToken"]);
            if (token.IsNullOrEmpty()) return token;

            var refreshToken = await _redisCache.GetAsync<string>(_configuration["MicrosoftGraphApi:CacheKeyForRefreshToken"]);

            if (string.IsNullOrEmpty(refreshToken))
                return null;

            var newToken = await GetTokenByRefreshTokenAsync(refreshToken);

            if (newToken == null || newToken.Access_token.IsNullOrEmpty())
                throw new Exception("MicrosoftTeams didn't generate new token maybe RefreshToken expired");

            await StoreTokenInCacheAsync(newToken);
            return newToken.Access_token;
        }
        private async Task<MicrosoftTemasUserModel> GetMeFromCacheAsync()
        {
            var cacheKey = $"currentteamagent";
            return await _redisCache.GetAsync<MicrosoftTemasUserModel>(cacheKey);
        }
        private async Task SaveMeCacheAsync(MicrosoftTemasUserModel me)
        {
            var cacheKey = $"currentteamagent";
            await _redisCache.SetAsync(cacheKey, me);
        }
        private async Task StoreTokenInCacheAsync(MicrosoftTeamsToken token)
        {
            if (token == null || token.Access_token.IsNullOrEmpty())
                throw new Exception("error in obtain access token");

            await _redisCache.SetAsync(
               _configuration["MicrosoftGraphApi:CacheKeyForToken"],
                token.Access_token);

            await _redisCache.SetAsync(
               _configuration["MicrosoftGraphApi:CacheKeyForRefreshToken"],
                token.Refresh_token);
        }
        private async Task<MicrosoftTeamsToken> GetTokenByRefreshTokenAsync(string refreshToken)
        {
            var redirectUrl = _redisCache.GetAsync<string>("MicrosofTeamsRedirectUrl").Result;
            if (redirectUrl.IsNullOrEmpty()) throw new Exception("RedirectUrl not found to obtain new token");

            var request = new Dictionary<string, string>
            {
                { "client_id", _configuration["MicrosoftGraphApi:ClientId"] },
                { "scope", _configuration["MicrosoftGraphApi:Scope"] },
                { "client_secret", _configuration["MicrosoftGraphApi:ClientSecret"] },
                { "grant_type", _configuration["MicrosoftGraphApi:RefreshTokenGrantType"] },
                { "refresh_token", refreshToken },
                { "redirect_uri", redirectUrl }
            };

            return await PostAsUrlEncodedAsync<MicrosoftTeamsToken>(request, _configuration["MicrosoftGraphApi:TokenUrl"]);
        }

        public async Task<T> GetAsync<T>(string url, string token)
        {
            var _client = new HttpClient();

            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _client.GetAsync(url);
            var resultAsString = await response.Content.ReadAsStringAsync();
            return PrepareResultType<T>(resultAsString);
        }


        private async Task<T> PostAsJsonAsync<T>(object obj, string url, string token)
        {
            var contentType = "application/json";
            var requestAsJson = "";
            var httpClient = new HttpClient();

            if (!string.IsNullOrEmpty(token))
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

            httpClient.BaseAddress = new Uri(url);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

            requestAsJson = JsonConvert.SerializeObject(obj);
            var response = await httpClient.SendAsync(GetRequestMessageForJson(requestAsJson, url, contentType));

            if (response.IsSuccessStatusCode)
            {
                var serviceResult = await response.Content.ReadAsStringAsync();
                return PrepareResultType<T>(serviceResult);
            }

            throw new Exception($"Post failed. {url} \n {response.ReasonPhrase}");
        }
        private async Task<T> PostAsUrlEncodedAsync<T>(object obj, string url)
        {
            HttpClient httpClient = new HttpClient();
            var contentType = "application/x-www-form-urlencoded";

            httpClient.BaseAddress = new Uri(url);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

            var response = await httpClient.SendAsync(GetRequestMessageForUrlEncoded(obj, url, contentType));

            if (response.IsSuccessStatusCode)
            {
                var serviceResult = await response.Content.ReadAsStringAsync();
                PrepareResultType<T>(serviceResult);
            }

            throw new Exception($"Post failed. {url} \n {response.ReasonPhrase}");
        }
        private T PrepareResultType<T>(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return default(T);

                if (typeof(T) == typeof(string))
                    return (T)Convert.ChangeType(str, typeof(T));

                return JsonConvert.DeserializeObject<T>(str);
            }
            catch (System.Exception)
            {
                throw new System.Exception(
                    $"error in Deserialize to {typeof(T).Name}.\n service result was : \n {str}");
            }
        }

        private HttpRequestMessage GetRequestMessageForUrlEncoded(object obj, string url, string contentType)
        {
            if (obj.GetType() != typeof(Dictionary<string, string>))
                throw new System.Exception($"can not create Dictionary<string, string> for {contentType} request.");

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new FormUrlEncodedContent((Dictionary<string, string>)obj);
            return request;
        }
        private HttpRequestMessage GetRequestMessageForJson(string requestAsJson, string url, string contentType)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(requestAsJson, Encoding.UTF8, contentType);
            return request;
        }
    }
}
