using RpaAlm.Shared.Configuration;
using RpaAlm.Shared.ApiClient.Core;
using RpaAlm.Shared.Models.DTOs;
using RpaAlm.Shared.Models.Requests.Create;
using RpaAlm.Shared.Models.Requests.Update;
using RpaAlm.Shared.Models.Responses;

namespace RpaAlm.Shared.ApiClient.Clients;

public class StatusApiClient
{
    private readonly ApiClient _apiClient;
    private const string Endpoint = "/api/status";

    public StatusApiClient()
    {
        _apiClient = new ApiClient(AppConfig.ApiBaseUrl);
    }

    public async Task<List<StatusDto>> GetAllAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<List<StatusDto>>>(Endpoint);
        return response?.Data ?? new List<StatusDto>();
    }

    public async Task<StatusDto?> GetByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<ApiResponse<StatusDto>>($"{Endpoint}/{id}");
        return response?.Data;
    }

    public async Task<StatusDto?> CreateAsync(StatusCreateRequest request)
    {
        var response = await _apiClient.PostAsync<StatusCreateRequest, ApiResponse<StatusDto>>(Endpoint, request);
        return response?.Data;
    }

    public async Task<StatusDto?> UpdateAsync(int id, StatusUpdateRequest request)
    {
        var response = await _apiClient.PutAsync<StatusUpdateRequest, ApiResponse<StatusDto>>($"{Endpoint}/{id}", request);
        return response?.Data;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiClient.DeleteAsync($"{Endpoint}/{id}");
    }
}
