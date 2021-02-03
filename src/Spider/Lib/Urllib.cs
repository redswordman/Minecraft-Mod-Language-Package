using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Spider.Lib.JsonLib;

namespace Spider.Lib {
    public static class UrlLib {
        /// <summary>
        /// 使用定义的游戏版本和模组数量批量获取模组信息
        /// </summary>
        /// <param name="modCount"></param>
        /// <param name="gameVersion"></param>
        /// <returns></returns>
        public static async Task<ModInfo[]> GetModInfoAsync(int modCount, string gameVersion)
        {
            using var httpClient = new HttpClient();
            var uriBuilder = new UriBuilder("https://addons-ecs.forgesvc.net/api/v2/addon/search")
            {
                Query =
                    $"categoryId=0&gameId=432&index=0&pageSize={modCount}&gameVersion={gameVersion}&sectionId=6&sort=1"
            };
            return await httpClient.GetFromJsonAsync<ModInfo[]>(uriBuilder.Uri);
        }

        /// <summary>
        /// 获取白名单模组的信息
        /// </summary>
        /// <param name="whiteList"></param>
        /// <returns></returns>
        public static async Task<ModInfo[]> GetModInfoAsync(string[] whiteList) {
            if (whiteList.ToList().Contains("null")) return Array.Empty<ModInfo>();
            var res = new List<ModInfo>();
            foreach (var id in whiteList) {
                using var httpClient = new HttpClient();
                var uri = new Uri($"https://addons-ecs.forgesvc.net/api/v2/addon/{id}");
                var result = await httpClient.GetFromJsonAsync<ModInfo>(uri);
                res.Add(result);
            }

            return res.ToArray();
        }

        /// <summary>
        /// 获取下载链接
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Uri GetDownloadUrl(string fileId, string fileName)
        {
            return new Uri($"https://edge.forgecdn.net/files/{fileId[..4]}/{fileId[4..]}/{fileName}");
        }

        /// <summary>
        /// 获取项目名称
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetProjectName(Uri uri) {
            var url = uri.ToString();
            var start = url.LastIndexOf('/') + 1;
            return url[start..];
        }
    }
}