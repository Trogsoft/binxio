using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Binxio.Abstractions
{
    public interface IXioLog<T>
    {
        void LogInformation(string message, params (string, string)[] context);
        void LogError(string message, params (string, string)[] context);
        void LogWarning(string message, params (string, string)[] context);
        void LogError(Exception ex, params (string, string)[] context);
        void DescribeOperation(string description);
        void OverrideUser(ClaimsPrincipal user);
    }
}
