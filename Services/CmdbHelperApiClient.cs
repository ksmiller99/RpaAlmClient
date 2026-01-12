using RpaAlmClient.Configuration;
using RpaAlmClient.Models;

namespace RpaAlmClient.Services;

public class CmdbHelperApiClient
{
    private readonly ApiClient _apiClient;
    private const string Endpoint = "/api/cmdbhelper";

    public CmdbHelperApiClient()
    {
        _apiClient = new ApiClient(AppConfig.ApiBaseUrl);
    }

    public async Task<List<CmdbHelperDto>> GetAllAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<List<CmdbHelperDto>>>(Endpoint);
        return response?.Data ?? new List<CmdbHelperDto>();
    }

    public async Task<CmdbHelperDto?> GetByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<ApiResponse<CmdbHelperDto>>("${Endpoint}/{id}");
        return response?.Data;
    }

    public async Task<CmdbHelperDto?> CreateAsync(CmdbHelperCreateRequest request)
    {
        var response = await _apiClient.PostAsync<CmdbHelperCreateRequest, ApiResponse<CmdbHelperDto>>(Endpoint, request);
        return response?.Data;
    }

    public async Task<CmdbHelperDto?> UpdateAsync(int id, CmdbHelperUpdateRequest request)
    {
        var response = await _apiClient.PutAsync<CmdbHelperUpdateRequest, ApiResponse<CmdbHelperDto>>("${Endpoint}/{id}", request);
        return response?.Data;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiClient.DeleteAsync("${Endpoint}/{id}");
    }
}
