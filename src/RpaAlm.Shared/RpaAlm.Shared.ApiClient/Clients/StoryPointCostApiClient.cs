using RpaAlm.Shared.Configuration;
using RpaAlm.Shared.ApiClient.Core;
using RpaAlm.Shared.Models.DTOs;
using RpaAlm.Shared.Models.Requests.Create;
using RpaAlm.Shared.Models.Requests.Update;
using RpaAlm.Shared.Models.Responses;

namespace RpaAlm.Shared.ApiClient.Clients;

public class StoryPointCostApiClient
{
    private readonly ApiClient _apiClient;
    private const string Endpoint = "/api/storypointcost";

    public StoryPointCostApiClient()
    {
        _apiClient = new ApiClient(AppConfig.ApiBaseUrl);
    }

    public async Task<List<StoryPointCostDto>> GetAllAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<List<StoryPointCostDto>>>(Endpoint);
        return response?.Data ?? new List<StoryPointCostDto>();
    }

    public async Task<StoryPointCostDto?> GetByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<ApiResponse<StoryPointCostDto>>($"{Endpoint}/{id}");
        return response?.Data;
    }

    public async Task<StoryPointCostDto?> CreateAsync(StoryPointCostCreateRequest request)
    {
        var response = await _apiClient.PostAsync<StoryPointCostCreateRequest, ApiResponse<StoryPointCostDto>>(Endpoint, request);
        return response?.Data;
    }

    public async Task<StoryPointCostDto?> UpdateAsync(int id, StoryPointCostUpdateRequest request)
    {
        var response = await _apiClient.PutAsync<StoryPointCostUpdateRequest, ApiResponse<StoryPointCostDto>>($"{Endpoint}/{id}", request);
        return response?.Data;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiClient.DeleteAsync($"{Endpoint}/{id}");
    }
}
