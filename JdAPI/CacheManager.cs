/****************************************************************************
The MIT License(MIT)

Copyright(c) 2016 Bohn Technology Solutions, LLC

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JdAPI.DataContracts;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Compat.Web;
using Newtonsoft.Json;

namespace JdAPI
{
    public class cacheItem
    {
        public object Value { get; set; }
        public DateTime Expiration { get; set; }
    }

    public static class CacheManager
    {
        private static Dictionary<string, cacheItem> cacheItems = new Dictionary<string, cacheItem>();

        private static void cleanCache()
        {
            var keysToRemove = new List<string>();
            foreach(var key in cacheItems.Keys)
            {
                var item = cacheItems[key];
                
                if (item.Expiration <= DateTime.Now)
                {
                    keysToRemove.Add(key);
                    item = null;
                }
            }

            foreach(var key in keysToRemove)
            {
                cacheItems.Remove(key);
            }
        }

        public static void AddCacheItem(string key, object value, int minutes)
        {
            cleanCache();

            cacheItem i = new cacheItem();
            i.Value = value;
            i.Expiration = DateTime.Now.AddMinutes(minutes);

            if (cacheItems.ContainsKey(key))
            {
                cacheItems[key].Value = value;
            }
            else
            {
                cacheItems.Add(key, i);
            }
        }

        public static object GetCacheItem(string key)
        {
            cleanCache();

            if (cacheItems.ContainsKey(key) && cacheItems[key] != null)
                return cacheItems[key].Value;
            else return null;
        }

        public static void Empty()
        {
            var keysToRemove = new List<string>();
            foreach (var key in cacheItems.Keys)
            {
                var item = cacheItems[key];
                keysToRemove.Add(key);
                item = null;
            }

            foreach (var key in keysToRemove)
            {
                cacheItems.Remove(key);
            }

            cacheItems = new Dictionary<string, cacheItem>();
        }

    }
}
