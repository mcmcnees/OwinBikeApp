using BikeMgrWeb.Models;
using IdentityModel.Client;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace BikeMgrWeb.Services
{
    public class HttpService
    {
        private Options _options;

        public HttpService(Options options)
        {
            _options = options;
        }

        public async Task<T> Get<T>(HttpContextBase context, string url)
        {
            var accessToken = await GetAccessTokenAsync(context);
            var client = new HttpClient();

            var requstUrl = new Uri(new Uri(_options.ApiUrl), url);
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, requstUrl);
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Value);
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.SendAsync(httpRequest);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);

        }

        public Task<U> Post<T, U>(HttpContextBase context, string url, T payload)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(payload));
            return Post<U>(context, url, content);
        }

        public async Task<T> Post<T>(HttpContextBase context, string url, HttpContent payload)
        {
            var accessToken = await GetAccessTokenAsync(context);
            var client = new HttpClient();

            var requstUrl = new Uri(new Uri(_options.ApiUrl), url);
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, requstUrl);
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Value);
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpRequest.Content = payload;

            var response = await client.SendAsync(httpRequest);

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public Task<U> Put<T, U>(HttpContextBase context, string url, T payload)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(payload));
            return Put<U>(context, url, content);
        }

        public async Task<T> Put<T>(HttpContextBase context, string url, HttpContent payload)
        {
            var accessToken = await GetAccessTokenAsync(context);
            var client = new HttpClient();

            var requstUrl = new Uri(new Uri(_options.ApiUrl), url);
            var httpRequest = new HttpRequestMessage(HttpMethod.Put, requstUrl);
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Value);
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpRequest.Content = payload;

            var response = await client.SendAsync(httpRequest);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<bool> Delete(HttpContextBase context, string url)
        {
            var accessToken = await GetAccessTokenAsync(context);
            var client = new HttpClient();

            var requstUrl = new Uri(new Uri(_options.ApiUrl), url);
            var httpRequest = new HttpRequestMessage(HttpMethod.Delete, requstUrl);
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Value);
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.SendAsync(httpRequest);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        public async Task<Token> GetAccessTokenAsync(HttpContextBase context)
        {
            var token = context.Session["access_token"] as Token;
            if (token != null && token.ExpiresAt > DateTime.Now)
                return token;

            var identity = context.User.Identity as ClaimsIdentity;
            if (identity == null) throw new Exception("User is not Authorized");

            //var idString = identity.FindFirst("id_token")?.Value;
            var accessString = identity.FindFirst("access_token")?.Value;
            var expiresAt = Convert.ToDateTime(identity.FindFirst("expires_at")?.Value);
            if (!String.IsNullOrEmpty(accessString) && expiresAt > DateTime.Now)
            {
                token = new Token
                {
                    Value = accessString,
                    ExpiresAt = expiresAt
                };
                return token;
            }
            var refresh = identity.FindFirst("refresh_token")?.Value;

            var response = await TokenClientExtensions.RequestRefreshTokenAsync(new TokenClient(ConfigurationManager.AppSettings["BikeApi.TokenUri"],
                ConfigurationManager.AppSettings["BikeApi.ClientID"], ConfigurationManager.AppSettings["BikeApi.Secret"]), refresh);
            token = new Token
            {
                Value = response.IdentityToken,
                ExpiresAt = DateTime.Now + TimeSpan.FromSeconds(response.ExpiresIn)
            };
            context.Session["access_token"] = token;
            return token;
        }

        public class Options
        {
            public string ApiUrl { get; set; }
        }
    }
}