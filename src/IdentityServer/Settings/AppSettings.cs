using System.Collections.Generic;

namespace IdentityServer
{
    /// <summary>
    /// Application settings
    /// </summary>
    public class AppSettings
    {
        public string ConnectionString { get; set; }
        public string AppinsightsIK { get; set; }
        public bool SeedData { get; set; }
        public string IdentityUrl { get; set; }
    }
}