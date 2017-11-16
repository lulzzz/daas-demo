using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DaaSDemo.KubeClient.Models
{
    /// <summary>
    ///     No description provided.
    /// </summary>
    public class CertificateSigningRequestStatusV1Beta1
    {
        /// <summary>
        ///     If request was approved, the controller will place the issued certificate here.
        /// </summary>
        [JsonProperty("certificate")]
        public string Certificate { get; set; }
    }
}
