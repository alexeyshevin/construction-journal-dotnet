using System.Net.Http.Json;
using ConstructionJournal.Web.Models;

namespace ConstructionJournal.Web.Services;

public class WorkLogsApiClient(HttpClient http)
{
    public async Task<List<WorkTypeDto>> GetWorkTypesAsync()
    {
        return await http.GetFromJsonAsync<List<WorkTypeDto>>("work-types") ?? [];
    }

    public async Task<List<WorkLogDto>> GetWorkLogsAsync(DateTime? dateFrom, DateTime? dateTo, string sort)
    {
        var query = new List<string>();

        if (dateFrom.HasValue)
            query.Add($"dateFrom={dateFrom:yyyy-MM-dd}");

        if (dateTo.HasValue)
            query.Add($"dateTo={dateTo:yyyy-MM-dd}");

        query.Add($"sort={sort}");

        var url = "work-logs?" + string.Join("&", query);

        return await http.GetFromJsonAsync<List<WorkLogDto>>(url) ?? [];
    }

    public async Task CreateAsync(WorkLogFormModel model)
    {
        await http.PostAsJsonAsync("work-logs", model);
    }

    public async Task UpdateAsync(Guid id, WorkLogFormModel model)
    {
        await http.PatchAsJsonAsync($"work-logs/{id}", model);
    }

    public async Task DeleteAsync(Guid id)
    {
        await http.DeleteAsync($"work-logs/{id}");
    }
}