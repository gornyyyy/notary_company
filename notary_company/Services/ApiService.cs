using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using notary_company.shared.Dtos;

namespace notary_company.Services
{
    public class ApiService : IDisposable
    {
        private readonly HttpClient _http;

        public ApiService(HttpClient http)
        {
            _http = http;
        }

        // ========== SERVICES ==========
        public async Task<List<ServiceDto>> GetServicesAsync()
        {
            return await _http.GetFromJsonAsync<List<ServiceDto>>("api/services")
                   ?? new List<ServiceDto>();
        }

        // ========== NOTARIES ==========
        public async Task<List<NotaryDto>> GetNotariesAsync()
        {
            return await _http.GetFromJsonAsync<List<NotaryDto>>("api/notaries")
                   ?? new List<NotaryDto>();
        }

        // ========== REQUESTS ==========
        public async Task<List<RequestDto>> GetRequestsAsync()
        {
            return await _http.GetFromJsonAsync<List<RequestDto>>("api/requests")
                   ?? new List<RequestDto>();
        }

        public async Task CreateRequestAsync(CreateRequestDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/requests", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateRequestStatusAsync(int requestId, UpdateStatusDto dto)
        {
            var response = await _http.PutAsJsonAsync($"api/requests/{requestId}/status", dto);
            response.EnsureSuccessStatusCode();
        }

        // ========== AUTH ==========
        public async Task<NotaryDto?> LoginAsync(string login, string password)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login",
                new LoginDto { Login = login, Password = password });

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<NotaryDto>();
        }

        public async Task AddHelperAsync(AddHelperDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/add-helper", dto);
            response.EnsureSuccessStatusCode();
        }

        public void Dispose() => _http?.Dispose();
    }
}