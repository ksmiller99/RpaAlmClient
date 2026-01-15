using RpaAlm.Shared.Configuration;
using RpaAlm.Shared.ApiClient.Core;
using RpaAlm.Shared.Models.DTOs;
using RpaAlm.Shared.Models.Requests.Create;
using RpaAlm.Shared.Models.Requests.Update;
using RpaAlm.Shared.Models.Responses;

namespace RpaAlm.Shared.ApiClient.Clients;

public class RpaStatusApiClient
{
    private readonly ApiClient _apiClient;
    private const string Endpoint = "/api/status";

    public RpaStatusApiClient()
    {
        _apiClient = new ApiClient(AppConfig.ApiBaseUrl);
    }

    public async Task<List<RpaStatusDto>> GetAllAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<List<RpaStatusDto>>>(Endpoint);
        return response?.Data ?? new List<RpaStatusDto>();
    }

    public async Task<RpaStatusDto?> GetByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<ApiResponse<RpaStatusDto>>($"{Endpoint}/{id}");
        return response?.Data;
    }

    public async Task<RpaStatusDto?> CreateAsync(RpaStatusCreateRequest request)
    {
        var response = await _apiClient.PostAsync<RpaStatusCreateRequest, ApiResponse<RpaStatusDto>>(Endpoint, request);
        return response?.Data;
    }

    public async Task<RpaStatusDto?> UpdateAsync(int id, RpaStatusUpdateRequest request)
    {
        var response = await _apiClient.PutAsync<RpaStatusUpdateRequest, ApiResponse<RpaStatusDto>>($"{Endpoint}/{id}", request);
        return response?.Data;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiClient.DeleteAsync($"{Endpoint}/{id}");
    }
}
