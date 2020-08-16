using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using Newtonsoft.Json;

namespace Speedy
{
    public class Speedy
    {
        private static readonly CookieContainer CookieJar = new CookieContainer();
        private static readonly HttpClientHandler handler = new HttpClientHandler()
        {
            CookieContainer = CookieJar
        };
        private static readonly HttpClient client = new HttpClient(handler);
        

        public Speedy()
        {
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.125 Safari/537.36");
            client.DefaultRequestHeaders.Referrer = new Uri("https://myspeedy.speedy.bg/");
        }

        public async Task<string> openLogin()
        {
            var page = "https://myspeedy.speedy.bg/";
            var response = await client.GetAsync(page);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

        public async Task<List<Address>> Search(string phoneNumber)
        {
            var uriBuilder = new UriBuilder("https://myspeedy.speedy.bg/rest/client/servingSite");
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["searchNonActive"] = "false";
            query["searchActive"] = "true";
            query["searchDomestic"] = "true";
            query["searchForeign"] = "true";
            query["localityId"] = "";
            query["phone"] = phoneNumber;
            query["name"] = "";
            query["object"] = "";
            query["type"] = "all";
            query["retReceiverId"] = "";
            query["distinctOffice"] = "false";
            query["sort"] = "";
            query["order"] = "asc";
            query["page"] = "1";
            query["pageSize"] = "10";
            uriBuilder.Query = query.ToString();
            var response = await client.GetAsync(uriBuilder.Uri);
            var responseString = await response.Content.ReadAsStringAsync();
            dynamic responseJSON = JsonConvert.DeserializeObject(responseString);
            var addresses = new List<Address>();
            foreach (var row in responseJSON.rows)
            {
                var addressOnRow = new Address();
                addressOnRow.FullAddress = row.mapUIFields.uiClientInfoAddress;
                addressOnRow.SpeedyType = row.type;
                var addressJson = row.address.data;
                addressOnRow.IsOffice = addressJson.officeToggle;
                addressOnRow.Country = addressJson.countryName;
                addressOnRow.Neighborhood = addressJson.quarterName;
                addressOnRow.PostalCode = addressJson.postCode;
                addressOnRow.City = addressJson.siteName;
                addressOnRow.SiteType = addressJson.siteType;
                addressOnRow.Latitude = addressJson.x;
                addressOnRow.Longitude = addressJson.y;
                addressOnRow.BlockNumber = addressJson.blockNo;
                addressOnRow.Municipality = row.address.locality.municipality.value;
                addressOnRow.Region = row.address.locality.region.value;
                addressOnRow.PhoneCode = row.address.locality.phoneCode;
                addressOnRow.FullRegion = row.address.locality.details.value;
                addresses.Add(addressOnRow);
            }
            return addresses;
        }

        public async Task<string> login(string userName, string password)
        {
            string loginPage = "https://myspeedy.speedy.bg/login";
            var values = new Dictionary<string, string>();
            values["originalURL"] = "/";
            values["captchaRequired"] = "false";
            values["j_username"] = userName;
            values["j_password"] = password;
            values["j_submit"] = "";

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(loginPage, content);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
    }
}
