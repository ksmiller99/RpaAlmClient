using RpaAlmClient.Configuration;
using RpaAlmClient.Models;

namespace RpaAlmClient.Services;

public class SlaItemApiClient
{
    private readonly ApiClient _apiClient;
    private const string Endpoint = "/api/slaitem";

    public SlaItemApiClient()
    {
        _apiClient = new ApiClient(AppConfig.ApiBaseUrl);
    }

    public async Task<List<SlaItemDto>> GetAllAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<List<SlaItemDto>>>(Endpoint);
        return response?.Data ?? new List<SlaItemDto>();
    }

    public async Task<SlaItemDto?> GetByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<ApiResponse<SlaItemDto>>("${Endpoint}/{id}");
        return response?.Data;
    }

    public async Task<SlaItemDto?> CreateAsync(SlaItemCreateRequest request)
    {
        var response = await _apiClient.PostAsync<SlaItemCreateRequest, ApiResponse<SlaItemDto>>(Endpoint, request);
        return response?.Data;
    }

    public async Task<SlaItemDto?> UpdateAsync(int id, SlaItemUpdateRequest request)
    {
        var response = await _apiClient.PutAsync<SlaItemUpdateRequest, ApiResponse<SlaItemDto>>("${Endpoint}/{id}", request);
        return response?.Data;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiClient.DeleteAsync("${Endpoint}/{id}");
    }
}
