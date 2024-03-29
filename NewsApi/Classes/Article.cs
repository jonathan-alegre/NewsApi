﻿namespace NewsApi.Classes
{
    [Serializable()]
    public class Article
    {
        public DateTime publishedAt { get; set; }
        public string urlToImage { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
    }
}
