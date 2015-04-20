using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class Program
    {
        static void Main(string[] args)
        {
            var intTask = GetIntAsync();

            if (intTask.Status == TaskStatus.RanToCompletion)
            {
                Console.WriteLine(intTask.Result);
            }
            else if (intTask.Status == TaskStatus.Canceled)
            {
                Console.WriteLine("任务被取消");
            }
            else
            {
                Console.WriteLine("发生错误哦");
                Console.WriteLine(intTask.Exception);
            }



            #region 2
            //var httpClient = new HttpClient();
            //Task<string> baiduTask = httpClient.GetStringAsync("http://www.baidu.com");

            //var httpClient2 = new HttpClient();
            //Task<string> sinaTask = httpClient2.GetStringAsync("http://www.sina.com.cn");

            ////等上面2个任务完成时这里再开始
            //Task<string[]> task = Task.WhenAll(baiduTask, sinaTask);

            //task.ContinueWith(stringArray =>
            //{
            //    //如果任务完成
            //    if (task.Status == TaskStatus.RanToCompletion)
            //    {
            //        for (int i = 0; i < stringArray.Result.Length;i++)
            //        {
            //            Console.WriteLine(stringArray.Result[i].Substring(0,100));
            //        }
            //    }
            //    else if (task.Status == TaskStatus.Canceled) //如果被取消
            //    {
            //        Console.WriteLine("{0}这个任务被取消了",task.Id);
            //    }
            //    else //发生错误
            //    {
            //        Console.WriteLine("发生错误了~~");
            //        foreach (var item in task.Exception.InnerExceptions)
            //        {
            //            Console.WriteLine(item.Message);
            //        }
            //    }
            //}); 
            #endregion

            #region 1
            //var doWorkTask = DoWorkAsync();
            //if (doWorkTask.IsCompleted)
            //{
            //    Console.WriteLine(doWorkTask.Result);
            //}
            //else
            //{
            //    doWorkTask.ContinueWith((pre) =>
            //    {
            //        Console.WriteLine(pre.Result);
            //    }, TaskContinuationOptions.NotOnFaulted);

            //    doWorkTask.ContinueWith((pre) =>
            //    {
            //        Console.WriteLine(pre.Exception);
            //    }, TaskContinuationOptions.OnlyOnFaulted);
            //}


            //Console.WriteLine("我会什么时候显示"); 
            #endregion
            Console.ReadKey();
        }

        static Task<int> GetIntAsync()
        {
            return Task.FromResult(10);
        }

        static Task<string> DoWorkAsync()
        {
            return Task<string>.Factory.StartNew(() =>
            {
                Thread.Sleep(3000);
                return "hello";
            });
        }
    }

    public static class AsyncFactory
    {
        public static Task<int> GetIntAsync()
        {
            var tsc = new TaskCompletionSource<int>();

            var timer = new System.Timers.Timer(2000);
            timer.AutoReset = false;
            timer.Elapsed += (s, e) =>
            {
                tsc.SetResult(10);
                timer.Dispose();
            };
            timer.Start();
            return tsc.Task;
        }
    }
}
