using RpaAlm.Shared.Configuration;
using RpaAlm.Shared.ApiClient.Core;
using RpaAlm.Shared.Models.DTOs;
using RpaAlm.Shared.Models.Requests.Create;
using RpaAlm.Shared.Models.Requests.Update;
using RpaAlm.Shared.Models.Responses;

namespace RpaAlm.Shared.ApiClient.Clients;

public class SegmentApiClient
{
    private readonly ApiClient _apiClient;
    private const string Endpoint = "/api/segment";

    public SegmentApiClient()
    {
        _apiClient = new ApiClient(AppConfig.ApiBaseUrl);
    }

    public async Task<List<SegmentDto>> GetAllAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<List<SegmentDto>>>(Endpoint);
        return response?.Data ?? new List<SegmentDto>();
    }

    public async Task<SegmentDto?> GetByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<ApiResponse<SegmentDto>>($"{Endpoint}/{id}");
        return response?.Data;
    }

    public async Task<SegmentDto?> CreateAsync(SegmentCreateRequest request)
    {
        var response = await _apiClient.PostAsync<SegmentCreateRequest, ApiResponse<SegmentDto>>(Endpoint, request);
        return response?.Data;
    }

    public async Task<SegmentDto?> UpdateAsync(int id, SegmentUpdateRequest request)
    {
        var response = await _apiClient.PutAsync<SegmentUpdateRequest, ApiResponse<SegmentDto>>($"{Endpoint}/{id}", request);
        return response?.Data;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiClient.DeleteAsync($"{Endpoint}/{id}");
    }
}
