using RpaAlmClient.Configuration;
using RpaAlmClient.Models;

namespace RpaAlmClient.Services;

public class VirtualIdentityApiClient
{
    private readonly ApiClient _apiClient;
    private const string Endpoint = "/api/virtualidentity";

    public VirtualIdentityApiClient()
    {
        _apiClient = new ApiClient(AppConfig.ApiBaseUrl);
    }

    public async Task<List<VirtualIdentityDto>> GetAllAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<List<VirtualIdentityDto>>>(Endpoint);
        return response?.Data ?? new List<VirtualIdentityDto>();
    }

    public async Task<VirtualIdentityDto?> GetByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<ApiResponse<VirtualIdentityDto>>("${Endpoint}/{id}");
        return response?.Data;
    }

    public async Task<VirtualIdentityDto?> CreateAsync(VirtualIdentityCreateRequest request)
    {
        var response = await _apiClient.PostAsync<VirtualIdentityCreateRequest, ApiResponse<VirtualIdentityDto>>(Endpoint, request);
        return response?.Data;
    }

    public async Task<VirtualIdentityDto?> UpdateAsync(int id, VirtualIdentityUpdateRequest request)
    {
        var response = await _apiClient.PutAsync<VirtualIdentityUpdateRequest, ApiResponse<VirtualIdentityDto>>("${Endpoint}/{id}", request);
        return response?.Data;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiClient.DeleteAsync("${Endpoint}/{id}");
    }
}
