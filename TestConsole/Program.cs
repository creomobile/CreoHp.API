using System;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace TestConsole
{
    class Program
    {
        static void CreateRsa()
        {
            var param = new RSACryptoServiceProvider(2048).ExportParameters(true);
            var jsonString = JsonConvert.SerializeObject(param);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
