using System;
using System.Text;
using System.Security.Cryptography;

namespace HTMLReaderCS.models
{
    public class HashGenerator
    {

        private HashGenerator() { }

        /// <summary>
        /// 文字列のMD5ハッシュを取得します。セキュリティに関わる用途では使用禁止。
        /// </summary>
        /// <param name="target"></param>
        /// <returns> MD5ハッシュを生成し、16進数表記(数字、アルファベット小文字)で32文字の文字列を取得します。</returns>
        public static string getMD5Hash(string target)
        {
            byte[] data = Encoding.UTF8.GetBytes(target);

            using (var md5 = new MD5CryptoServiceProvider())
            {
                byte[] bs = md5.ComputeHash(data);
                md5.Clear();
                return BitConverter.ToString(bs).ToLower().Replace("-", "").ToString();
            }
        }
    }
}
