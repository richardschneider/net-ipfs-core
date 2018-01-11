using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipfs.CoreApi
{
    /// <summary>
    ///   Manages the IPLD Directed Acrylic Graph.
    /// </summary>
    /// <remarks>
    ///   The dag API is a replacement of the <see cref="IObjectApi"/>, which only supported 'dag-pb'.
    ///   This API supports other IPLD formats, such as dag-cbor, ethereum-block, git, ...
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
        ///   The data to send to the network.
        /// </param>
        /// <param name="contentType">
        ///   The content type or format of the <paramref name="data"/>; such as "raw" or "dag-db".
        ///   See <see cref="MultiCodec"/> for more details.
        /// </param>
        /// <param name="multiHash">
        ///   The <see cref="MultiHash"/> algorithm name used to produce the <see cref="Cid"/>.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///    A task that represents the asynchronous put operation. The task's value is
        ///    the data's <see cref="Cid"/>.
        /// </returns>
        Task<Cid> PutAsync(
            ILinkedNode data,
            string contentType,
            string multiHash = MultiHash.DefaultAlgorithmName,
            CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Get an IPLD node.
        /// </summary>
        /// <param name="path">
        ///   The CID or path to an IPLD node.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous get operation. The task's value
        ///   contains the node's content.
        /// </returns>
        Task<ILinkedNode> GetAsync(string path, CancellationToken cancel = default(CancellationToken));
    }
}
