using Binxio.Abstractions;
using Binxio.Common;
using Binxio.Management.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Binxio.Management.Web.Services
{
    public class XioLog<T> : IXioLog<T>
    {

        private readonly ILogger<T> ncLog;
        private readonly IHttpContextAccessor httpcx;
        private readonly ILogWriter logWriter;

        public string OperationId { get; }

        public string LocalOperationId { get; } = Guid.NewGuid().ToString();

        private string ToBase36String(byte[] toConvert, bool bigEndian = false)
        {
            const string alphabet = "0123456789abcdefghijklmnopqrstuvwxyz";
            if (bigEndian) Array.Reverse(toConvert); // !BitConverter.IsLittleEndian might be an alternative
            BigInteger dividend = new BigInteger(toConvert);
            var builder = new StringBuilder();
            while (dividend != 0)
            {
                BigInteger remainder;
                dividend = BigInteger.DivRem(dividend, 36, out remainder);
                builder.Insert(0, alphabet[Math.Abs(((int)remainder))]);
            }
            return builder.ToString();
        }

        public XioLog(ILogger<T> ncLog, IHttpContextAccessor httpcx, ILogWriter logWriter)
        {
            this.ncLog = ncLog;
            this.httpcx = httpcx;
            this.logWriter = logWriter;
            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(httpcx.HttpContext.TraceIdentifier);
                byte[] hash = sha.ComputeHash(textData);
                this.OperationId = ToBase36String(hash);
            }
        }

        private string Description;

        public void DescribeOperation(string description)
        {
            this.Description = description;
        }

        private void writeLog(XioLogLevel level, string message, params (string, string)[] context)
        {
            logWriter.Write(new XioLogEntry
            {
                ContextId = LocalOperationId,
                Level = level,
                IsContextTitle = false,
                Message = message,
                Time = DateTime.Now,
                UserId = httpcx.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                OperationId = OperationId,
                Context = context.ToDictionary(x => x.Item1, x => x.Item2)
            });
        }

        public void LogError(string message, params (string, string)[] context)
        {
            ncLog.LogError(message, context.Select(x => $"{x.Item1}={x.Item2}"));
            writeLog(XioLogLevel.Error, message, context);
        }

        public void LogError(Exception ex, params (string, string)[] context)
        {
            ncLog.LogError(ex, ex.Message, context.Select(x => $"{x.Item1}={x.Item2}"));
            writeLog(XioLogLevel.Error, ex.Message, context);
        }

        public void LogInformation(string message, params (string, string)[] context)
        {
            ncLog.LogInformation(message, context.Select(x => $"{x.Item1}={x.Item2}"));
            writeLog(XioLogLevel.Information, message, context);
        }

        public void LogWarning(string message, params (string, string)[] context)
        {
            ncLog.LogWarning(message, context.Select(x => $"{x.Item1}={x.Item2}"));
            writeLog(XioLogLevel.Warning, message, context);
        }

        public XioResult GetResult(bool success) => new XioResult(success) { OperationId = this.OperationId };
        public XioResult<R> GetResult<R>(bool success) => new XioResult<R>(success);
        public XioResult GetResult(bool success, string message) => new XioResult(success, message);
        public XioResult<R> GetResult<R>(bool success, string message) => new XioResult<R>(success, message);
        public XioResult<R> GetResult<R>(bool success, R payload) => new XioResult<R>(success, payload);
        public XioResult<R> GetResult<R>(bool success, string message, R payload) => new XioResult<R>(success, message, payload);

    }
}
