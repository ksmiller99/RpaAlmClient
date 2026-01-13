using RpaAlm.Shared.Configuration;
using RpaAlm.Shared.ApiClient.Core;
using RpaAlm.Shared.Models.DTOs;
using RpaAlm.Shared.Models.Requests.Create;
using RpaAlm.Shared.Models.Requests.Update;
using RpaAlm.Shared.Models.Responses;

namespace RpaAlm.Shared.ApiClient.Clients;

public class MedalApiClient
{
    private readonly ApiClient _apiClient;
    private const string Endpoint = "/api/medal";

    public MedalApiClient()
    {
        _apiClient = new ApiClient(AppConfig.ApiBaseUrl);
    }

    public async Task<List<MedalDto>> GetAllAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<List<MedalDto>>>(Endpoint);
        return response?.Data ?? new List<MedalDto>();
    }

    public async Task<MedalDto?> GetByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<ApiResponse<MedalDto>>("${Endpoint}/{id}");
        return response?.Data;
    }

    public async Task<MedalDto?> CreateAsync(MedalCreateRequest request)
    {
        var response = await _apiClient.PostAsync<MedalCreateRequest, ApiResponse<MedalDto>>(Endpoint, request);
        return response?.Data;
    }

    public async Task<MedalDto?> UpdateAsync(int id, MedalUpdateRequest request)
    {
        var response = await _apiClient.PutAsync<MedalUpdateRequest, ApiResponse<MedalDto>>("${Endpoint}/{id}", request);
        return response?.Data;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiClient.DeleteAsync("${Endpoint}/{id}");
    }
}
