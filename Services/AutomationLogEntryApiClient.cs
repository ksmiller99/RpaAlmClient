using RpaAlmClient.Configuration;
using RpaAlmClient.Models;

namespace RpaAlmClient.Services;

public class AutomationLogEntryApiClient
{
    private readonly ApiClient _apiClient;
    private const string Endpoint = "/api/automationlogentry";

    public AutomationLogEntryApiClient()
    {
        _apiClient = new ApiClient(AppConfig.ApiBaseUrl);
    }

    public async Task<List<AutomationLogEntryDto>> GetAllAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<List<AutomationLogEntryDto>>>(Endpoint);
        return response?.Data ?? new List<AutomationLogEntryDto>();
    }

    public async Task<AutomationLogEntryDto?> GetByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<ApiResponse<AutomationLogEntryDto>>("${Endpoint}/{id}");
        return response?.Data;
    }

    public async Task<AutomationLogEntryDto?> CreateAsync(AutomationLogEntryCreateRequest request)
    {
        var response = await _apiClient.PostAsync<AutomationLogEntryCreateRequest, ApiResponse<AutomationLogEntryDto>>(Endpoint, request);
        return response?.Data;
    }

    public async Task<AutomationLogEntryDto?> UpdateAsync(int id, AutomationLogEntryUpdateRequest request)
    {
        var response = await _apiClient.PutAsync<AutomationLogEntryUpdateRequest, ApiResponse<AutomationLogEntryDto>>("${Endpoint}/{id}", request);
        return response?.Data;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiClient.DeleteAsync("${Endpoint}/{id}");
    }
}
