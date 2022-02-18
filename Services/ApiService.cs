using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Newtonsoft.Json;

namespace Services {
    public class ApiService : IApiService {
        public async Task<IEnumerable<T>> GetDataList<T>(string url) {
            using var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (response.StatusCode != HttpStatusCode.OK) {
                return default;
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(responseBody)) {
                return default;
            }
            var model = JsonConvert.DeserializeObject<IEnumerable<T>>(responseBody);
            return model;
        }

        public async Task<T> GetDataSingle<T>(string url) {
            try {
                using var client = new HttpClient();
                var response = await client.GetAsync(url);
                if (response.StatusCode != HttpStatusCode.OK) {
                    return default;
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(responseBody)) {
                    return default;
                }
                var model = JsonConvert.DeserializeObject<T>(responseBody);
                return model;
            }
            catch (Exception) {
                return default;
            }
        }

        public async Task<string> GetSingle(string url) {
            try {
                using var client = new HttpClient();
                var response = await client.GetAsync(url);
                if (response.StatusCode != HttpStatusCode.OK) {
                    return default;
                }
                var responseBody = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(responseBody)) {
                    return default;
                }
                return responseBody;

            }
            catch (Exception) {
                return default;
            }
        }

        public async Task<TS> Post<T, TS>(string url, T model) {
            try {
                using var client = new HttpClient();

                var data = JsonConvert.SerializeObject(model);
                var stringContent = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, stringContent);
                if (response.StatusCode != HttpStatusCode.OK) {
                    return default;
                }

                var result = response.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrWhiteSpace(result)) {
                    return default;
                }
                var returnResponse = JsonConvert.DeserializeObject<TS>(result);
                return returnResponse;
            }
            catch (Exception) {
                return default;
            }
        }
        public async Task<TS> Delete<T, TS>(string url, T model) {
            try {
                using var client = new HttpClient();
                var data = JsonConvert.SerializeObject(model);
                var response = await client.SendAsync(
                      new HttpRequestMessage(HttpMethod.Delete, url) {
                          Content = new StringContent(data, Encoding.UTF8, "application/json")
                      }
                );

                if (response.StatusCode != HttpStatusCode.OK) {
                    return default;
                }

                var result = response.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrWhiteSpace(result)) {
                    return default;
                }

                var returnResponse = JsonConvert.DeserializeObject<TS>(result);
                return returnResponse;
            }
            catch (Exception) {
                return default;
            }
        }

        public async Task<TS> Delete<TS>(string url) {
            try {
                using var client = new HttpClient();
                var response = await client.SendAsync(
                      new HttpRequestMessage(HttpMethod.Delete, url) { }
                );

                if (response.StatusCode != HttpStatusCode.OK) {
                    return default;
                }

                var result = response.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrWhiteSpace(result)) {
                    return default;
                }
                var returnResponse = JsonConvert.DeserializeObject<TS>(result);
                return returnResponse;
            }
            catch (Exception) {
                return default;
            }
        }
    }
}