using RpaAlm.Shared.Configuration;
using RpaAlm.Shared.ApiClient.Core;
using RpaAlm.Shared.Models.DTOs;
using RpaAlm.Shared.Models.Requests.Create;
using RpaAlm.Shared.Models.Requests.Update;
using RpaAlm.Shared.Models.Responses;

namespace RpaAlm.Shared.ApiClient.Clients;

public class EnhancementApiClient
{
    private readonly ApiClient _apiClient;
    private const string Endpoint = "/api/enhancement";

    public EnhancementApiClient()
    {
        _apiClient = new ApiClient(AppConfig.ApiBaseUrl);
    }

    public async Task<List<EnhancementDto>> GetAllAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<List<EnhancementDto>>>(Endpoint);
        return response?.Data ?? new List<EnhancementDto>();
    }

    public async Task<EnhancementDto?> GetByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<ApiResponse<EnhancementDto>>("${Endpoint}/{id}");
        return response?.Data;
    }

    public async Task<EnhancementDto?> CreateAsync(EnhancementCreateRequest request)
    {
        var response = await _apiClient.PostAsync<EnhancementCreateRequest, ApiResponse<EnhancementDto>>(Endpoint, request);
        return response?.Data;
    }

    public async Task<EnhancementDto?> UpdateAsync(int id, EnhancementUpdateRequest request)
    {
        var response = await _apiClient.PutAsync<EnhancementUpdateRequest, ApiResponse<EnhancementDto>>("${Endpoint}/{id}", request);
        return response?.Data;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiClient.DeleteAsync("${Endpoint}/{id}");
    }
}
