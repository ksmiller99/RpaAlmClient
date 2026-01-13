using RpaAlm.Shared.Configuration;
using RpaAlm.Shared.ApiClient.Core;
using RpaAlm.Shared.Models.DTOs;
using RpaAlm.Shared.Models.Requests.Create;
using RpaAlm.Shared.Models.Requests.Update;
using RpaAlm.Shared.Models.Responses;

namespace RpaAlm.Shared.ApiClient.Clients;

public class EnhancementUserStoryApiClient
{
    private readonly ApiClient _apiClient;
    private const string Endpoint = "/api/enhancementuserstory";

    public EnhancementUserStoryApiClient()
    {
        _apiClient = new ApiClient(AppConfig.ApiBaseUrl);
    }

    public async Task<List<EnhancementUserStoryDto>> GetAllAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<List<EnhancementUserStoryDto>>>(Endpoint);
        return response?.Data ?? new List<EnhancementUserStoryDto>();
    }

    public async Task<EnhancementUserStoryDto?> GetByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<ApiResponse<EnhancementUserStoryDto>>("${Endpoint}/{id}");
        return response?.Data;
    }

    public async Task<EnhancementUserStoryDto?> CreateAsync(EnhancementUserStoryCreateRequest request)
    {
        var response = await _apiClient.PostAsync<EnhancementUserStoryCreateRequest, ApiResponse<EnhancementUserStoryDto>>(Endpoint, request);
        return response?.Data;
    }

    public async Task<EnhancementUserStoryDto?> UpdateAsync(int id, EnhancementUserStoryUpdateRequest request)
    {
        var response = await _apiClient.PutAsync<EnhancementUserStoryUpdateRequest, ApiResponse<EnhancementUserStoryDto>>("${Endpoint}/{id}", request);
        return response?.Data;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiClient.DeleteAsync("${Endpoint}/{id}");
    }
}
