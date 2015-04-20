using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine(DownloadStringWithRetries("/api/Product/1").Result);
            Console.WriteLine(DownloadStringWithTimeout("/api/Product/1").Result);
            //Task<int> t = Task.Factory.StartNew(() => GetSth(2));
            //Console.WriteLine(DelayResult(t.Result, TimeSpan.FromSeconds(5)).Result);
            Console.ReadKey();
        }

        static async Task<T> DelayResult<T>(T result, TimeSpan delay)
        {
            await Task.Delay(delay);
            return result;
        }

        static int GetSth(int a)
        {
            return a;
        }

        //尝试4次，一秒钟后重试，然后2秒钟后重试，再然后4秒钟后重试，最后尝试一次
        static async Task<string> DownloadStringWithRetries(string uri)
        {
           
            using (var client = new HttpClient())
            {
                //设置
                client.BaseAddress = new Uri("http://localhost:1310/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //一秒钟后重试，然后2秒钟后重试，再然后4秒钟后重试
                var nextDelay = TimeSpan.FromSeconds(1);

                for (int i = 0; i != 3; ++i)
                {
                    try
                    {
                        return await client.GetStringAsync(uri);
                    }
                    catch 
                    {
                    }
                    await Task.Delay(nextDelay);
                    nextDelay = nextDelay + nextDelay;
                }

                //最后再试一次
                return await client.GetStringAsync(uri);
            }
        }

        //尝试2次，尝试一次后，再隔一段时间尝试
        static async Task<string> DownloadStringWithTimeout(string uri)
        {
            using (var client = new HttpClient())
            {
                //设置
                client.BaseAddress = new Uri("http://localhost:1310/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var downloadTask = client.GetStringAsync(uri);
                var timeoutTask = Task.Delay(3000);

                var completedTask = await Task.WhenAny(downloadTask, timeoutTask);

                //如果Web API服务没有在3秒响应，就返回null
                if (completedTask == timeoutTask)
                {
                    return null;
                }
                return await downloadTask;
            }
        }
    }
}
