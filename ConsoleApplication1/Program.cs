using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();
            Console.ReadKey();
        }

        static async Task RunAsync()
        {
            using (var client = new HttpClient())
            {
                //设置
                client.BaseAddress = new Uri("http://localhost:1310/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //添加 HTTP POST
                var myProduct = new Product() { Name = "myproduct", Price = 100, Category = "other" };
                HttpResponseMessage response = await client.PostAsJsonAsync("api/product", myProduct);
                if (response.IsSuccessStatusCode)
                {
                    Uri pUrl = response.Headers.Location;

                    //修改 HTTP PUT
                    myProduct.Price = 80;   // Update price
                    response = await client.PutAsJsonAsync(pUrl, myProduct);

                    //删除 HTTP DELETE
                    response = await client.DeleteAsync(pUrl);
                }

                //异步获取数据
                response = await client.GetAsync("/api/Product");
                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<Product> products = await response.Content.ReadAsAsync<IEnumerable<Product>>();
                    foreach (var item in products)
                    {
                        Console.WriteLine("{0}\t{1}元\t{2}", item.Name, item.Price, item.Category);
                    }
                    
                }
            }
        }
    }
}
