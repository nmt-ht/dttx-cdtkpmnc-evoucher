using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;

namespace eVoucher.Handlers
{
    public static class RestClient
    {
        private static readonly string authSchema = "Basic";
        public static eVoucherConfig VoucherConfig { get; set; } = new eVoucherConfig();

        private static ApiRestClient _apiClient;
        public static ApiRestClient APIClient
        {
            get
            {
                if (_apiClient == null)
                {
                    HttpClient _httpClient = null;

                    if (VoucherConfig != null)
                    {
                        _httpClient = new HttpClient
                        {
                            BaseAddress = new Uri(VoucherConfig.eVoucherServerEndPoint),
                            Timeout = TimeSpan.FromMinutes(60)
                        };
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authSchema, $" {VoucherConfig.BasicAuth}");
                    }

                    _apiClient = new ApiRestClient(_httpClient);
                }

                return _apiClient;
            }
        }
    }

    public class ApiRestClient
    {
        private readonly HttpClient _httpClient;

        public ApiRestClient(HttpClient client)
        {
            _httpClient = client;
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<TValue> GetAsync<TValue>(string url)
        {
            TValue result;
            try
            {
                if (string.IsNullOrEmpty(url)) result = default;

                using var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    using var strResult = await response.Content.ReadAsStreamAsync();
                    result = await StreamSerializer.DeserializeAsync<TValue>(strResult);
                }
                else result = default;
            }
            catch
            {
                throw;
            }

            return result;
        }

        public async Task<ApiResponse> GetAsync(string url)
        {
            ApiResponse result;
            try
            {
                result = await this.GetAsync<ApiResponse>(url);
            }
            catch (Exception ex)
            {
                result = new ApiResponse(false, ex.ToString());
            }

            return result;
        }

        public async Task<ApiResponse> PostAsync(string url, object parameter)
        {
            ApiResponse result;
            try
            {
                var json = JsonConvert.SerializeObject(parameter);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                using var requestResponse = await _httpClient.PostAsync(url, data);

                if (requestResponse.IsSuccessStatusCode || requestResponse.StatusCode == System.Net.HttpStatusCode.NotAcceptable)
                {
                    using var strResult = await requestResponse.Content.ReadAsStreamAsync();
                    result = await StreamSerializer.DeserializeAsync<ApiResponse>(strResult);
                }
                else
                {
                    result = new ApiResponse(false, StatusCodes.Status500InternalServerError, "Internal Server Error");
                }
            }
            catch (HttpRequestException ex)
            {
                result = new ApiResponse(false, ex.ToString());
            }

            return result;
        }

        public async Task<ApiResponse> PutAsync(string url, object parameter)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return null;

                var json = JsonConvert.SerializeObject(parameter);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var requestResponse = await _httpClient.PutAsync(url, data);

                if (requestResponse.IsSuccessStatusCode)
                {
                    return new ApiResponse(true, StatusCodes.Status200OK, "Success");
                }
            }
            catch (HttpRequestException ex)
            {
                return new ApiResponse(false, StatusCodes.Status500InternalServerError, ex.ToString());
            }

            return new ApiResponse(false, StatusCodes.Status500InternalServerError, "Internal Server Error");
        }

        public async Task<ApiResponse> DeleteAsync(string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return null;

                var requestResponse = await _httpClient.DeleteAsync(url);

                if (requestResponse.IsSuccessStatusCode || requestResponse.StatusCode == System.Net.HttpStatusCode.NotAcceptable)
                    return JsonConvert.DeserializeObject<ApiResponse>(requestResponse.Content.ReadAsStringAsync().Result);
            }
            catch (HttpRequestException ex)
            {
                return new ApiResponse(false, ex.ToString());
            }

            return new ApiResponse(false, StatusCodes.Status500InternalServerError, "Internal Server Error");
        }
    }
}
