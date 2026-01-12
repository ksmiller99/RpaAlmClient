using RpaAlmClient.Configuration;
using RpaAlmClient.Models;

namespace RpaAlmClient.Services;

public class ViAssignmentsApiClient
{
    private readonly ApiClient _apiClient;
    private const string Endpoint = "/api/viassignments";

    public ViAssignmentsApiClient()
    {
        _apiClient = new ApiClient(AppConfig.ApiBaseUrl);
    }

    public async Task<List<ViAssignmentsDto>> GetAllAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<List<ViAssignmentsDto>>>(Endpoint);
        return response?.Data ?? new List<ViAssignmentsDto>();
    }

    public async Task<ViAssignmentsDto?> GetByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<ApiResponse<ViAssignmentsDto>>("${Endpoint}/{id}");
        return response?.Data;
    }

    public async Task<ViAssignmentsDto?> CreateAsync(ViAssignmentsCreateRequest request)
    {
        var response = await _apiClient.PostAsync<ViAssignmentsCreateRequest, ApiResponse<ViAssignmentsDto>>(Endpoint, request);
        return response?.Data;
    }

    public async Task<ViAssignmentsDto?> UpdateAsync(int id, ViAssignmentsUpdateRequest request)
    {
        var response = await _apiClient.PutAsync<ViAssignmentsUpdateRequest, ApiResponse<ViAssignmentsDto>>("${Endpoint}/{id}", request);
        return response?.Data;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiClient.DeleteAsync("${Endpoint}/{id}");
    }
}
