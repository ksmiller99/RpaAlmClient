using RpaAlm.Shared.Configuration;
using RpaAlm.Shared.ApiClient.Core;
using RpaAlm.Shared.Models.DTOs;
using RpaAlm.Shared.Models.Requests.Create;
using RpaAlm.Shared.Models.Requests.Update;
using RpaAlm.Shared.Models.Responses;

namespace RpaAlm.Shared.ApiClient.Clients;

public class SlaMasterApiClient
{
    private readonly ApiClient _apiClient;
    private const string Endpoint = "/api/slamaster";

    public SlaMasterApiClient()
    {
        _apiClient = new ApiClient(AppConfig.ApiBaseUrl);
    }

    public async Task<List<SlaMasterDto>> GetAllAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<List<SlaMasterDto>>>(Endpoint);
        return response?.Data ?? new List<SlaMasterDto>();
    }

    public async Task<SlaMasterDto?> GetByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<ApiResponse<SlaMasterDto>>("${Endpoint}/{id}");
        return response?.Data;
    }

    public async Task<SlaMasterDto?> CreateAsync(SlaMasterCreateRequest request)
    {
        var response = await _apiClient.PostAsync<SlaMasterCreateRequest, ApiResponse<SlaMasterDto>>(Endpoint, request);
        return response?.Data;
    }

    public async Task<SlaMasterDto?> UpdateAsync(int id, SlaMasterUpdateRequest request)
    {
        var response = await _apiClient.PutAsync<SlaMasterUpdateRequest, ApiResponse<SlaMasterDto>>("${Endpoint}/{id}", request);
        return response?.Data;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiClient.DeleteAsync("${Endpoint}/{id}");
    }
}
