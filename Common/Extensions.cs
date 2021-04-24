namespace Bring2mind.Backup.Restore.Common
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using System.Text;

    public static class Extensions
    {
        public static string EnsureEndsWith(this string input, string endCharacter)
        {
            if (!input.EndsWith(endCharacter))
            {
                input += endCharacter;
            }
            return input;
        }

        public static T GetResponse<T>(this HttpWebResponse input)
        {
            var responseString = new StreamReader(input.GetResponseStream()).ReadToEnd();
            return JsonConvert.DeserializeObject<T>(responseString);
        }

        public static string ToQueryString(this NameValueCollection input)
        {
            return input.ToQueryString(true);
        }
        public static string ToQueryString(this NameValueCollection input, bool addQuestionMark)
        {
            if (input == null) return "";
            if (input.Count == 0) return "";
            var res = addQuestionMark ? "?" : "";
            var newParams = new List<string>();
            foreach (var nv in input.AllKeys)
            {
                newParams.Add(nv + "=" + System.Web.HttpUtility.UrlEncode(input[nv]));
            }
            return res + string.Join("&", newParams);
        }

        public static string ToEncodedString(this Stream stream, Encoding enc = null)
        {
            var content = "";
            using (var sr = new StreamReader(stream))
            {
                content = sr.ReadToEnd();
            }
            return content;
        }

        public static DirectoryInfo EnsureExists(this string folderPath)
        {
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            return new DirectoryInfo(folderPath);
        }
    }
}
