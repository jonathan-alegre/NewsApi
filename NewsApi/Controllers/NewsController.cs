using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NewsApi.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net.Http;

namespace NewsApi.Controllers
{
    [Route( "api/news" )]
  [ApiController]
  public class NewsController : ControllerBase
  {
    const string apiKey = "0d91cbee476b443a8ba43c4f1667da9c";
    const string language = "es";    

    [Route( "top-headlines" )]
    [HttpGet]
    public async Task<List<Article>> TopHeadLines
    (
      string country,
      int page,
      int pageSize
    )
    {
      string url = "https://newsapi.org/v2/top-headlines?country=" + country +
                   "&pageSize=" + pageSize +
                   "&page=" + page +
                   "&apiKey=" + apiKey;      

      HttpClient client = new HttpClient();
      client.DefaultRequestHeaders.UserAgent.ParseAdd( "Mozilla/5.0 (compatible; AcmeInc/1.0)" );
      var response = await client.GetAsync( url );
      string responseBody = await response.Content.ReadAsStringAsync();
      var data = JsonConvert.DeserializeObject( responseBody );

      var jsonArticles = data != null ? ( (JObject)data ).Children<JProperty>().FirstOrDefault( x => x.Name == "articles" ) : null;

      var articles = new List<Article>();

      if ( jsonArticles != null )
      {
        articles = JsonConvert.DeserializeObject<List<Article>>( jsonArticles.First().ToString() );
      }

      return articles;
    }

    [Route( "search" )]
    [HttpGet]
    public async Task<List<Article>> Search
    (
      string dateFrom,
      string dateTo,
      string keyWords,
      int page,
      int pageSize
    )
    {
      string url = "https://newsapi.org/v2/everything?language=" + language +                   
                   ( !string.IsNullOrEmpty( dateFrom ) ? "&from=" + DateTime.Parse( dateFrom ).ToString("yyyy-MM-dd")  : string.Empty ) +
                   ( !string.IsNullOrEmpty( dateTo ) ? "&to=" + DateTime.Parse( dateTo ).ToString( "yyyy-MM-dd" ) : string.Empty ) +
                   "&q=" + keyWords +
                   "&page=" + page +
                   "&pageSize=" + pageSize +
                   "&sortBy=publishedAt" +
                   "&apiKey=" + apiKey;
      
      HttpClient client = new HttpClient();
      client.DefaultRequestHeaders.UserAgent.ParseAdd( "Mozilla/5.0 (compatible; AcmeInc/1.0)" );
      var response = await client.GetAsync( url );
      string responseBody = await response.Content.ReadAsStringAsync();
      var data = JsonConvert.DeserializeObject( responseBody );

      var jsonArticles = data != null ? ( (JObject)data ).Children<JProperty>().FirstOrDefault( x => x.Name == "articles" ) : null;

      var articles = new List<Article>();

      if ( jsonArticles != null )
      {
        articles = JsonConvert.DeserializeObject<List<Article>>( jsonArticles.First().ToString() );
      }

      return articles;
    }
  }
}
