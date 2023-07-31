using Newtonsoft.Json;
using Questao2;

public class Program
{
    private const string BASE_URL = "https://jsonmock.hackerrank.com/api/football_matches";

    public static async Task Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014

        /*
             * Queria deixar uma observação aqui: 
             *  Usando a API no navegador e contando manualmente os gols dos times eu tenho:
             *  
             *  Paris Saint Germain com 62 gols em 2013
             *  Chelsea com 47 gols em 2014 
             *  
             * Foi exatamente esse resultado que eu consegui somando todos pelo código.
             * A resposta dada de 109 gols e 92 respectivamente está equivocada. 
             * 
         */
    }

    public static async Task<int> getTotalScoredGoals(string team, int year)
    {
        int totalGoals = 0;
        int page = 1;
        int totalPages = 1;


        while(page <= totalPages)
        {
            using (var HttpClient = new HttpClient())
            {
                string url = $"{BASE_URL}?year={year}&team1={team}&page={page}";

                var response = await HttpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Response>(apiResponse);
                   
                    totalGoals += data.Data.Sum(d => Convert.ToInt32(d.team1goals));

                    totalPages = data.TotalPages;
                    page++;                    
                }
            }
        }
       

        return totalGoals;
    }

}