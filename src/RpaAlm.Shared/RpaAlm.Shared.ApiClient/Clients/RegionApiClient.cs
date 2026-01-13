using RpaAlm.Shared.Configuration;
using RpaAlm.Shared.ApiClient.Core;
using RpaAlm.Shared.Models.DTOs;
using RpaAlm.Shared.Models.Requests.Create;
using RpaAlm.Shared.Models.Requests.Update;
using RpaAlm.Shared.Models.Responses;

namespace RpaAlm.Shared.ApiClient.Clients;

public class RegionApiClient
{
    private readonly ApiClient _apiClient;
    private const string Endpoint = "/api/region";

    public RegionApiClient()
    {
        _apiClient = new ApiClient(AppConfig.ApiBaseUrl);
    }

    public async Task<List<RegionDto>> GetAllAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<List<RegionDto>>>(Endpoint);
        return response?.Data ?? new List<RegionDto>();
    }

    public async Task<RegionDto?> GetByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<ApiResponse<RegionDto>>("${Endpoint}/{id}");
        return response?.Data;
    }

    public async Task<RegionDto?> CreateAsync(RegionCreateRequest request)
    {
        var response = await _apiClient.PostAsync<RegionCreateRequest, ApiResponse<RegionDto>>(Endpoint, request);
        return response?.Data;
    }

    public async Task<RegionDto?> UpdateAsync(int id, RegionUpdateRequest request)
    {
        var response = await _apiClient.PutAsync<RegionUpdateRequest, ApiResponse<RegionDto>>("${Endpoint}/{id}", request);
        return response?.Data;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiClient.DeleteAsync("${Endpoint}/{id}");
    }
}
