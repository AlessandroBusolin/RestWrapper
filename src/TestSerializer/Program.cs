﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using RestWrapper;

namespace TestSerializer
{
    internal class Program
    {
        // Thank you https://random-data-api.com/documentation
        static string _ApiUrl = "https://random-data-api.com/api/stripe/random_stripe";
        static HttpMethod _Method = HttpMethod.GET;
        static bool _UseDefaultSerializer = true;

        static void Main(string[] args)
        {
            RestRequest req = new RestRequest(_ApiUrl, _Method);
            RestResponse resp = req.Send();
            if (!_UseDefaultSerializer)
            {
                resp.SerializationHelper = new Serializer();
            }
            else
            {
                Console.WriteLine("Deserializing using default serializer");
            }
            Dictionary<string, object> data = resp.DataFromJson<Dictionary<string, object>>();
            foreach (KeyValuePair<string, object> kv in data)
            {
                Console.WriteLine(kv.Key + ": " + kv.Value.ToString());
            }
        }
    }

    internal class Serializer : ISerializationHelper
    {
        public T DeserializeJson<T>(string json) where T : class, new()
        {
            Console.WriteLine("Deserializing using Newtonsoft.JSON");
            return JsonConvert.DeserializeObject<T>(json);
        }

        public string SerializeJson(object obj, bool pretty)
        {
            Console.WriteLine("Serializing using Newtonsoft.JSON");

            if (pretty)
            {
                return JsonConvert.SerializeObject(obj, Formatting.Indented);
            }
            else
            {
                return JsonConvert.SerializeObject(obj);
            }
        }
    }
}
