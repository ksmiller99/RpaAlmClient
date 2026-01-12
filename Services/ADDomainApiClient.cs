using RpaAlmClient.Configuration;
using RpaAlmClient.Models;

namespace RpaAlmClient.Services;

public class ADDomainApiClient
{
    private readonly ApiClient _apiClient;
    private const string Endpoint = "/api/addomain";

    public ADDomainApiClient()
    {
        _apiClient = new ApiClient(AppConfig.ApiBaseUrl);
    }

    public async Task<List<ADDomainDto>> GetAllAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<List<ADDomainDto>>>(Endpoint);
        return response?.Data ?? new List<ADDomainDto>();
    }

    public async Task<ADDomainDto?> GetByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<ApiResponse<ADDomainDto>>("${Endpoint}/{id}");
        return response?.Data;
    }

    public async Task<ADDomainDto?> CreateAsync(ADDomainCreateRequest request)
    {
        var response = await _apiClient.PostAsync<ADDomainCreateRequest, ApiResponse<ADDomainDto>>(Endpoint, request);
        return response?.Data;
    }

    public async Task<ADDomainDto?> UpdateAsync(int id, ADDomainUpdateRequest request)
    {
        var response = await _apiClient.PutAsync<ADDomainUpdateRequest, ApiResponse<ADDomainDto>>("${Endpoint}/{id}", request);
        return response?.Data;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _apiClient.DeleteAsync("${Endpoint}/{id}");
    }
}
