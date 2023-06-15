using System.Collections.Generic;
using System.Security.Policy;

namespace eVoucher.Pages
{
    public class CountryInfo
    {

        public int id { get; set; }
        public string Name { get; set; }
        public int TotalCases { get; set; }
        public int TotalDeaths { get; set; }
        public string DeathPercentage => (this.TotalDeaths * 100) / this.TotalCases + "%";

        public List<CountryInfo> GetCountryInfos()
        {
            var countryInfos = new List<CountryInfo>();
            countryInfos.Add(new CountryInfo() { id = 1, Name = "USA", TotalCases = 142178, TotalDeaths = 2484 });
            countryInfos.Add(new CountryInfo() { id = 1, Name = "Italy", TotalCases = 97689, TotalDeaths = 10779 });
            countryInfos.Add(new CountryInfo() { id = 1, Name = "China", TotalCases = 81470, TotalDeaths = 3304 });
            countryInfos.Add(new CountryInfo() { id = 1, Name = "Spain", TotalCases = 80110, TotalDeaths = 6803 });
            countryInfos.Add(new CountryInfo() { id = 1, Name = "Germany", TotalCases = 62435, TotalDeaths = 541 });
            countryInfos.Add(new CountryInfo() { id = 1, Name = "France", TotalCases = 40174, TotalDeaths = 2606 });
            countryInfos.Add(new CountryInfo() { id = 1, Name = "Iran", TotalCases = 38309, TotalDeaths = 2640 });

            return countryInfos;
        }

    }
}
