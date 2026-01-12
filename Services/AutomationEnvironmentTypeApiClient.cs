using RpaAlmClient.Configuration;
using RpaAlmClient.Models;

namespace RpaAlmClient.Services;

public class AutomationEnvironmentTypeApiClient
{
    private readonly ApiClient _apiClient;
    private const string Endpoint = "/api/automationenvironmenttype";

    public AutomationEnvironmentTypeApiClient()
    {
        _apiClient = new ApiClient(AppConfig.ApiBaseUrl);
    }

    public async Task<List<AutomationEnvironmentTypeDto>> GetAllAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<List<AutomationEnvironmentTypeDto>>>(Endpoint);
        return response?.Data ?? new List<AutomationEnvironmentTypeDto>();
    }

    public async Task<AutomationEnvironmentTypeDto?> GetByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<ApiResponse<AutomationEnvironmentTypeDto>>("${Endpoint}/{id}");
        return response?.Data;
    }

    public async Task<AutomationEnvironmentTypeDto?> CreateAsync(AutomationEnvironmentTypeCreateRequest request)
    {
        var response = await _apiClient.PostAsync<AutomationEnvironmentTypeCreateRequest, ApiResponse<AutomationEnvironmentTypeDto>>(Endpoint, request);
        return response?.Data;
    }

    public async Task<AutomationEnvironmentTypeDto?> UpdateAsync(int id, AutomationEnvironmentTypeUpdateRequest request)
    {
        var response = await _apiClient.PutAsync<AutomationEnvironmentTypeUpdateRequest, ApiResponse<AutomationEnvironmentTypeDto>>("${Endpoint}/{id}", request);
        return response?.Data;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiClient.DeleteAsync("${Endpoint}/{id}");
    }
}
