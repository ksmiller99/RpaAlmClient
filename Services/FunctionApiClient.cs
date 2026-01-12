using RpaAlmClient.Configuration;
using RpaAlmClient.Models;

namespace RpaAlmClient.Services;

public class FunctionApiClient
{
    private readonly ApiClient _apiClient;
    private const string Endpoint = "/api/function";

    public FunctionApiClient()
    {
        _apiClient = new ApiClient(AppConfig.ApiBaseUrl);
    }

    public async Task<List<FunctionDto>> GetAllAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<List<FunctionDto>>>(Endpoint);
        return response?.Data ?? new List<FunctionDto>();
    }

    public async Task<FunctionDto?> GetByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<ApiResponse<FunctionDto>>("${Endpoint}/{id}");
        return response?.Data;
    }

    public async Task<FunctionDto?> CreateAsync(FunctionCreateRequest request)
    {
        var response = await _apiClient.PostAsync<FunctionCreateRequest, ApiResponse<FunctionDto>>(Endpoint, request);
        return response?.Data;
    }

    public async Task<FunctionDto?> UpdateAsync(int id, FunctionUpdateRequest request)
    {
        var response = await _apiClient.PutAsync<FunctionUpdateRequest, ApiResponse<FunctionDto>>("${Endpoint}/{id}", request);
        return response?.Data;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiClient.DeleteAsync("${Endpoint}/{id}");
    }
}
