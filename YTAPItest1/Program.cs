using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Nancy.Json;
using Newtonsoft.Json.Linq;

namespace HttpClientSample
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }

        public string item { get; set; }
    }


    public class Id
    {
        public string videoId { get; set; }

        public  Id()
        {
            videoId = "hi";
        }
        
    }

    public class Items
    {
        public Id id { get; set; }

        public Items()
        {
            id = new Id();
        }

    }

    public class MyResp
    {
        public IList<Items> items { get; set; }

        public MyResp()
        {
            
        }

    }

    class Program
    {
        static HttpClient client = new HttpClient();

        static void ShowProduct(MyResp myresp)
        {
            Console.WriteLine($"Name: {myresp.items[0].id.videoId} ");
        }

        static async Task<Uri> CreateProductAsync(Product product)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/products", product);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        static async Task<MyResp> GetProductAsync(string path)
        {
            MyResp myresp = new MyResp();
            HttpResponseMessage response = await client.GetAsync("youtube/v3/search?part=snippet&channelId=UCPXBKQxddbDqwr5G8kTaKVg&maxResults=1&order=date&key=AIzaSyB9DJyO6YCKnmllKkYLuykCMbSqTkwRK-A&type=video");
            if (response.IsSuccessStatusCode)
            {
                //string temp = response.Content.ToString();
                //Console.Write(temp);

                //JObject search = JObject.Parse(temp);

                //JToken results = search["regionCode"];







                //items = results.ToObject<Product>();
                myresp = await response.Content.ReadAsAsync<MyResp>();



                //product = await response.Content.ReadAsAsync<Product>();

                //string data = await response.Content.ReadAsStringAsync();
                //use JavaScriptSerializer from System.Web.Script.Serialization
                //JavaScriptSerializer JSserializer = new JavaScriptSerializer();
                //deserialize to your class
                //product = JSserializer.Deserialize<Product>(data);
            }
            return myresp;
        }

        static async Task<Product> UpdateProductAsync(Product product)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"api/products/{product.Id}", product);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            product = await response.Content.ReadAsAsync<Product>();
            return product;
        }

        static async Task<HttpStatusCode> DeleteProductAsync(string id)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                $"api/products/{id}");
            return response.StatusCode;
        }

        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("https://www.googleapis.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Create a new product
                MyResp myresp = new MyResp();

              /*  VideoInfo vI = new VideoInfo()
                {
                    videoId = "g";
                }; */

                var url = new Uri("https://www.googleapis.com/youtube/v3/search?part=snippet&channelId=UCPXBKQxddbDqwr5G8kTaKVg&maxResults=1&order=date&key=AIzaSyB9DJyO6YCKnmllKkYLuykCMbSqTkwRK-A&type=video");
                Console.WriteLine($"Created at {url}");

                // Get the product
                myresp = await GetProductAsync(url.PathAndQuery);
                ShowProduct(myresp);

               /* // Update the product
                Console.WriteLine("Updating price...");
                product.Price = 80;
                await UpdateProductAsync(product);

                // Get the updated product
                product = await GetProductAsync(url.PathAndQuery);
                ShowProduct(product);

                // Delete the product
                var statusCode = await DeleteProductAsync(product.Id);
                Console.WriteLine($"Deleted (HTTP Status = {(int)statusCode})"); */

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}