using RpaAlm.Shared.Configuration;
using RpaAlm.Shared.ApiClient.Core;
using RpaAlm.Shared.Models.DTOs;
using RpaAlm.Shared.Models.Requests.Create;
using RpaAlm.Shared.Models.Requests.Update;
using RpaAlm.Shared.Models.Responses;

namespace RpaAlm.Shared.ApiClient.Clients;

public class AutomationEnvironmentApiClient
{
    private readonly ApiClient _apiClient;
    private const string Endpoint = "/api/automationenvironment";

    public AutomationEnvironmentApiClient()
    {
        _apiClient = new ApiClient(AppConfig.ApiBaseUrl);
    }

    public async Task<List<AutomationEnvironmentDto>> GetAllAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<List<AutomationEnvironmentDto>>>(Endpoint);
        return response?.Data ?? new List<AutomationEnvironmentDto>();
    }

    public async Task<AutomationEnvironmentDto?> GetByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<ApiResponse<AutomationEnvironmentDto>>("${Endpoint}/{id}");
        return response?.Data;
    }

    public async Task<AutomationEnvironmentDto?> CreateAsync(AutomationEnvironmentCreateRequest request)
    {
        var response = await _apiClient.PostAsync<AutomationEnvironmentCreateRequest, ApiResponse<AutomationEnvironmentDto>>(Endpoint, request);
        return response?.Data;
    }

    public async Task<AutomationEnvironmentDto?> UpdateAsync(int id, AutomationEnvironmentUpdateRequest request)
    {
        var response = await _apiClient.PutAsync<AutomationEnvironmentUpdateRequest, ApiResponse<AutomationEnvironmentDto>>("${Endpoint}/{id}", request);
        return response?.Data;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiClient.DeleteAsync("${Endpoint}/{id}");
    }
}
