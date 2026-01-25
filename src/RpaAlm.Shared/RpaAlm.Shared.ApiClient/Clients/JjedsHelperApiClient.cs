using RpaAlm.Shared.Configuration;
using RpaAlm.Shared.ApiClient.Core;
using RpaAlm.Shared.Models.DTOs;
using RpaAlm.Shared.Models.Requests.Create;
using RpaAlm.Shared.Models.Requests.Update;
using RpaAlm.Shared.Models.Responses;

namespace RpaAlm.Shared.ApiClient.Clients;

public class JjedsHelperApiClient
{
    private readonly ApiClient _apiClient;
    private const string Endpoint = "/api/jjedshelper";

    public JjedsHelperApiClient()
    {
        _apiClient = new ApiClient(AppConfig.ApiBaseUrl);
    }

    public async Task<List<JjedsHelperDto>> GetAllAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<List<JjedsHelperDto>>>(Endpoint);
        return response?.Data ?? new List<JjedsHelperDto>();
    }

    public async Task<JjedsHelperDto?> GetByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<ApiResponse<JjedsHelperDto>>($"{Endpoint}/{id}");
        return response?.Data;
    }

    public async Task<JjedsHelperDto?> CreateAsync(JjedsHelperCreateRequest request)
    {
        var response = await _apiClient.PostAsync<JjedsHelperCreateRequest, ApiResponse<JjedsHelperDto>>(Endpoint, request);
        return response?.Data;
    }

    public async Task<JjedsHelperDto?> UpdateAsync(int id, JjedsHelperUpdateRequest request)
    {
        var response = await _apiClient.PutAsync<JjedsHelperUpdateRequest, ApiResponse<JjedsHelperDto>>($"{Endpoint}/{id}", request);
        return response?.Data;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiClient.DeleteAsync($"{Endpoint}/{id}");
    }
}
