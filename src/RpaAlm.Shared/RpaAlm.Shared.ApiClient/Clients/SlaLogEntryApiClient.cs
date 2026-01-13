using RpaAlm.Shared.Configuration;
using RpaAlm.Shared.ApiClient.Core;
using RpaAlm.Shared.Models.DTOs;
using RpaAlm.Shared.Models.Requests.Create;
using RpaAlm.Shared.Models.Requests.Update;
using RpaAlm.Shared.Models.Responses;

namespace RpaAlm.Shared.ApiClient.Clients;

public class SlaLogEntryApiClient
{
    private readonly ApiClient _apiClient;
    private const string Endpoint = "/api/slalogentry";

    public SlaLogEntryApiClient()
    {
        _apiClient = new ApiClient(AppConfig.ApiBaseUrl);
    }

    public async Task<List<SlaLogEntryDto>> GetAllAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<List<SlaLogEntryDto>>>(Endpoint);
        return response?.Data ?? new List<SlaLogEntryDto>();
    }

    public async Task<SlaLogEntryDto?> GetByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<ApiResponse<SlaLogEntryDto>>("${Endpoint}/{id}");
        return response?.Data;
    }

    public async Task<SlaLogEntryDto?> CreateAsync(SlaLogEntryCreateRequest request)
    {
        var response = await _apiClient.PostAsync<SlaLogEntryCreateRequest, ApiResponse<SlaLogEntryDto>>(Endpoint, request);
        return response?.Data;
    }

    public async Task<SlaLogEntryDto?> UpdateAsync(int id, SlaLogEntryUpdateRequest request)
    {
        var response = await _apiClient.PutAsync<SlaLogEntryUpdateRequest, ApiResponse<SlaLogEntryDto>>("${Endpoint}/{id}", request);
        return response?.Data;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiClient.DeleteAsync("${Endpoint}/{id}");
    }
}
