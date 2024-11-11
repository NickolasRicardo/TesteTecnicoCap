using System.Net.Http.Json;
using Newtonsoft.Json;
using Questao2;

public class Program
{
    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static int getTotalScoredGoals(string team, int year)
    {
        HttpClient httpClient = new HttpClient();
        var baseUrl = "https://jsonmock.hackerrank.com/api/football_matches";
        int page = 1;
        int totalPages = 0;
        int totalScoredGoals = 0;

        do
        {
            var url = $"{baseUrl}?";
            url += $"year={year}&";
            url += $"page={page}&";
            url += $"team1={Uri.EscapeDataString(team)}";

            var response = httpClient.GetFromJsonAsync<footballMatchResponse>(url).Result;

            if (response == null)
                throw new NullReferenceException();

            totalScoredGoals += response.Data.Sum(x => int.Parse(x.Team1Goals));
            totalPages = response.TotalPages;
            page++;
        } while (page <= totalPages);

        page = 1;

        do
        {
            var url = $"{baseUrl}?";
            url += $"year={year}&";
            url += $"page={page}&";
            url += $"team2={Uri.EscapeDataString(team)}";

            var response = httpClient.GetFromJsonAsync<footballMatchResponse>(url).Result;

            if (response == null)
                throw new NullReferenceException();

            totalScoredGoals += response.Data.Sum(x => int.Parse(x.Team2Goals));
            totalPages = response.TotalPages;
            page++;
        } while (page <= totalPages);

        return totalScoredGoals;
    }
}