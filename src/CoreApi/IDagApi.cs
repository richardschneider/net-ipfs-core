using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipfs.CoreApi
{
    /// <summary>
    ///   Manages the IPLD (linked data) Directed Acrylic Graph.
    /// </summary>
    /// <remarks>
    ///   The DAG API is a replacement of the <see cref="IObjectApi"/>, which only supported 'dag-pb'.
    ///   This API supports other IPLD formats, such as cbor, ethereum-block, git, ...
    /// </remarks>
    /// <seealso cref="IObjectApi"/>
    /// <seealso cref="ILinkedNode"/>
    /// <seealso href="https://github.com/ipfs/interface-ipfs-core/blob/master/SPEC/DAG.md">Dag API spec</seealso>
    public interface IDagApi
    {
        /// <summary>
        ///  Put an IPLD node.
        /// </summary>
        /// <param name="data">
        ///   The JSON data to send to the network.
        /// </param>
        /// <param name="contentType">
        ///   The content type or format of the <paramref name="data"/>; such as "raw" or "cbor".
        ///   See <see cref="MultiCodec"/> for more details.  Defaults to "cbor".
        /// </param>
        /// <param name="multiHash">
        ///   The <see cref="MultiHash"/> algorithm name used to produce the <see cref="Cid"/>.
        /// </param>
        /// <param name="pin">
        ///   If <b>true</b> the <paramref name="data"/> is pinned to local storage and will not be
        ///   garbage collected.  The default is <b>true</b>.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous put operation. The task's value is
        ///   the data's <see cref="Cid"/>.
        /// </returns>
        Task<Cid> PutAsync(
            JObject data,
            string contentType = "cbor",
            string multiHash = MultiHash.DefaultAlgorithmName,
            bool pin = true,
            CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Get an IPLD node.
        /// </summary>
        /// <param name="id">
        ///   The <see cref="Cid"/> of the IPLD node.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous get operation. The task's value
        ///   contains the node's content as JSON.
        /// </returns>
        Task<JObject> GetAsync(Cid id, CancellationToken cancel = default(CancellationToken));
    }
}
