﻿using QiniuSdk.Util;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace QiniuSdk.Util
{
    /// <summary>
    ///     计算hash值
    ///     特别注意，不同平台使用的Cryptography可能略有不同，使用中如有遇到问题，请反馈
    ///     提交您的issue到 https://github.com/qiniu/csharp-sdk
    /// </summary>
    public class Hashing
    {
        /// <summary>
        ///     计算SHA1
        /// </summary>
        /// <param name="data">字节数据</param>
        /// <returns>SHA1</returns>
        public static byte[] CalcSha1(byte[] data)
        {
#if WINDOWS_UWP
            var sha = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha1);
            var buf = CryptographicBuffer.CreateFromByteArray(data);
            var digest = sha.HashData(buf);
            var hashBytes = new byte[digest.Length];
            CryptographicBuffer.CopyToByteArray(digest, out hashBytes);
            return hashBytes;
#else
            var sha1 = SHA1.Create();
            return sha1.ComputeHash(data);
#endif
        }

        /// <summary>
        ///     计算MD5哈希(可能需要关闭FIPS)
        /// </summary>
        /// <param name="str">待计算的字符串</param>
        /// <returns>MD5结果</returns>
        public static string CalcMd5(string str)
        {
#if WINDOWS_UWP
            var md5 = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            var buf = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf8);
            var digest = md5.HashData(buf);
            return CryptographicBuffer.EncodeToHexString(digest);
#else
            var md5 = MD5.Create();
            var data = Encoding.UTF8.GetBytes(str);
            var hashData = md5.ComputeHash(data);
            var sb = new StringBuilder(hashData.Length * 2);
            foreach (var b in hashData) sb.Append($"{b:x2}");
            return sb.ToString();
#endif
        }

        /// <summary>
        ///     计算MD5哈希(第三方实现)
        /// </summary>
        /// <param name="str">待计算的字符串,避免FIPS-Exception</param>
        /// <returns>MD5结果</returns>
        public static string CalcMd5X(string str)
        {
            var data = Encoding.UTF8.GetBytes(str);
            var md5 = new LabMd5();
            return md5.ComputeHash(data);
        }
    }
}
