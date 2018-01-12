using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipfs.CoreApi
{
    /// <summary>
    ///   Manages the IPFS Configuration.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///   Configuration values are JSON.  <see href="http://www.newtonsoft.com/json">Json.NET</see>
    ///   is used to represent JSON.
    ///   </para>
    /// </remarks>
    /// <seealso href="https://github.com/ipfs/interface-ipfs-core/blob/master/SPEC/CONFIG.md">Config API spec</seealso>
    public interface IConfigApi
    {
        /// <summary>
        ///   Gets the entire configuration.
        /// </summary>
        /// <returns>
        ///   A <see cref="JObject"/> containing the configuration.
        /// </returns>
        Task<JObject> GetAsync(CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Gets the value of a configuration key.
        /// </summary>
        /// <param name="key">
        ///   The key name, such as "Addresses.API".
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   The value of the <paramref name="key"/> as <see cref="JToken"/>.
        /// </returns>
        /// <exception cref="Exception">
        ///   When the <paramref name="key"/> does not exist.
        /// </exception>
        /// <remarks>
        ///   Keys are case sensistive.
        /// </remarks>
        Task<JToken> GetAsync(string key, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Adds or replaces a configuration value.
        /// </summary>
        /// <param name="key">
        ///   The key name, such as "Addresses.API".
        /// </param>
        /// <param name="value">
        ///   The new <see cref="string"/> value of the <paramref name="key"/>.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        Task SetAsync(string key, string value, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Adds or replaces a configuration value.
        /// </summary>
        /// <param name="key">
        ///   The key name, such as "Addresses.API".
        /// </param>
        /// <param name="value">
        ///   The new <see cref="JToken">JSON</see> value of the <paramref name="key"/>.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        Task SetAsync(string key, JToken value, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Replaces the entire configuration.
        /// </summary>
        /// <param name="config"></param>
        Task ReplaceAsync(JObject config);
    }
}
