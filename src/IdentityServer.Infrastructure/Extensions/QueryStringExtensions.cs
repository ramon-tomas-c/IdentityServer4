using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Http
{
    /// <summary>
    /// Helper to generate query strings
    /// </summary>
    public static class QueryStringExtensions
    {
        public static string FormatQueryString(this IQueryCollection queryParams)
        {
            return string.Join("&", queryParams.Select(q => $"{q.Key}={WebUtility.UrlEncode(q.Value)}"));
        }
    }
}
