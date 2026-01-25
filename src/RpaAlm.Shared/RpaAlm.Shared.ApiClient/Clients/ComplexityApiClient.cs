using RpaAlm.Shared.Configuration;
using RpaAlm.Shared.ApiClient.Core;
using RpaAlm.Shared.Models.DTOs;
using RpaAlm.Shared.Models.Requests.Create;
using RpaAlm.Shared.Models.Requests.Update;
using RpaAlm.Shared.Models.Responses;

namespace RpaAlm.Shared.ApiClient.Clients;

public class ComplexityApiClient
{
    private readonly ApiClient _apiClient;
    private const string Endpoint = "/api/complexity";

    public ComplexityApiClient()
    {
        _apiClient = new ApiClient(AppConfig.ApiBaseUrl);
    }

    public async Task<List<ComplexityDto>> GetAllAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<List<ComplexityDto>>>(Endpoint);
        return response?.Data ?? new List<ComplexityDto>();
    }

    public async Task<ComplexityDto?> GetByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<ApiResponse<ComplexityDto>>($"{Endpoint}/{id}");
        return response?.Data;
    }

    public async Task<ComplexityDto?> CreateAsync(ComplexityCreateRequest request)
    {
        var response = await _apiClient.PostAsync<ComplexityCreateRequest, ApiResponse<ComplexityDto>>(Endpoint, request);
        return response?.Data;
    }

    public async Task<ComplexityDto?> UpdateAsync(int id, ComplexityUpdateRequest request)
    {
        var response = await _apiClient.PutAsync<ComplexityUpdateRequest, ApiResponse<ComplexityDto>>($"{Endpoint}/{id}", request);
        return response?.Data;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiClient.DeleteAsync($"{Endpoint}/{id}");
    }
}
