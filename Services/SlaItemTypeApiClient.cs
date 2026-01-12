using RpaAlmClient.Configuration;
using RpaAlmClient.Models;

namespace RpaAlmClient.Services;

public class SlaItemTypeApiClient
{
    private readonly ApiClient _apiClient;
    private const string Endpoint = "/api/slaitemtype";

    public SlaItemTypeApiClient()
    {
        _apiClient = new ApiClient(AppConfig.ApiBaseUrl);
    }

    public async Task<List<SlaItemTypeDto>> GetAllAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<List<SlaItemTypeDto>>>(Endpoint);
        return response?.Data ?? new List<SlaItemTypeDto>();
    }

    public async Task<SlaItemTypeDto?> GetByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<ApiResponse<SlaItemTypeDto>>("${Endpoint}/{id}");
        return response?.Data;
    }

    public async Task<SlaItemTypeDto?> CreateAsync(SlaItemTypeCreateRequest request)
    {
        var response = await _apiClient.PostAsync<SlaItemTypeCreateRequest, ApiResponse<SlaItemTypeDto>>(Endpoint, request);
        return response?.Data;
    }

    public async Task<SlaItemTypeDto?> UpdateAsync(int id, SlaItemTypeUpdateRequest request)
    {
        var response = await _apiClient.PutAsync<SlaItemTypeUpdateRequest, ApiResponse<SlaItemTypeDto>>("${Endpoint}/{id}", request);
        return response?.Data;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiClient.DeleteAsync("${Endpoint}/{id}");
    }
}
