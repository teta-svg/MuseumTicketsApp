using Microsoft.AspNetCore.WebUtilities;
using Museum.Domain.DTOs;

namespace Museum.Blazor.Components.Pages
{
    public partial class Exhibitions
    {
        private List<PublicExhibitionDTO>? exhibitions;
        private ExhibitionFilterDto filter = new();

        protected override async Task OnInitializedAsync() => await LoadExhibitions();

        private async Task LoadExhibitions()
        {
            try
            {
                var queryParams = new Dictionary<string, string?>();

                if (!string.IsNullOrWhiteSpace(filter.Name)) queryParams.Add("Name", filter.Name);
                if (!string.IsNullOrWhiteSpace(filter.MuseumName)) queryParams.Add("MuseumName", filter.MuseumName);
                if (filter.MinPrice.HasValue) queryParams.Add("MinPrice", filter.MinPrice.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                if (filter.MaxPrice.HasValue) queryParams.Add("MaxPrice", filter.MaxPrice.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                if (filter.StartDate.HasValue) queryParams.Add("StartDate", filter.StartDate.Value.ToString("yyyy-MM-dd"));
                if (filter.EndDate.HasValue) queryParams.Add("EndDate", filter.EndDate.Value.ToString("yyyy-MM-dd"));

                var url = QueryHelpers.AddQueryString("api/Exhibitions/filter", queryParams);

                var result = await Https.GetFromJsonAsync<List<PublicExhibitionDTO>>(url);

                exhibitions = result ?? new();

                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] ОШИБКА В МЕТОДЕ: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"[INNER ERROR] {ex.InnerException.Message}");
            }
        }

        private async Task ResetFilters()
        {
            filter = new ExhibitionFilterDto();
            await LoadExhibitions();
        }
    }
}
