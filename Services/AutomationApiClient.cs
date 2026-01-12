using RpaAlmClient.Configuration;
using RpaAlmClient.Models;

namespace RpaAlmClient.Services;

public class AutomationApiClient
{
    private readonly ApiClient _apiClient;
    private const string Endpoint = "/api/automation";

    public AutomationApiClient()
    {
        _apiClient = new ApiClient(AppConfig.ApiBaseUrl);
    }

    public async Task<List<AutomationDto>> GetAllAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<List<AutomationDto>>>(Endpoint);
        return response?.Data ?? new List<AutomationDto>();
    }

    public async Task<AutomationDto?> GetByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<ApiResponse<AutomationDto>>("${Endpoint}/{id}");
        return response?.Data;
    }

    public async Task<AutomationDto?> CreateAsync(AutomationCreateRequest request)
    {
        var response = await _apiClient.PostAsync<AutomationCreateRequest, ApiResponse<AutomationDto>>(Endpoint, request);
        return response?.Data;
    }

    public async Task<AutomationDto?> UpdateAsync(int id, AutomationUpdateRequest request)
    {
        var response = await _apiClient.PutAsync<AutomationUpdateRequest, ApiResponse<AutomationDto>>("${Endpoint}/{id}", request);
        return response?.Data;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiClient.DeleteAsync("${Endpoint}/{id}");
    }
}
