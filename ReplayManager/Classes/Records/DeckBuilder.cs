using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ReplayManager.Classes.Records
{
    public class DeckBuilderData
    {
        public string PlayerName { get; set; }
        public string GameVersion { get;set; }
        public int CivId { get; set; }
        public DeckBuilderDeck Deck { get; set; }


        public async Task<string> PushData()
        {
            using (HttpClient httpClient = new HttpClient())
                using (HttpResponseMessage response = await httpClient.PostAsJsonAsync("http://www.rusages.ru/", this))
                {
                    response.EnsureSuccessStatusCode();
                    string res = await response.Content.ReadAsStringAsync();
                    return res;
                }
            }
        }
    

    public class DeckBuilderDeck
    {
        public string Name { get; set; }
        public List<DeckBuilderCard> Cards { get; set; }
    }

    public class DeckBuilderCard
    {
        public string Name { get; set; }
        public int MaxCount { get; set; }
        public string DiplayUnitCount { get; set; }
        public int Age { get; set; }
    }
}
