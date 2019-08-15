using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipfs.CoreApi
{
    /// <summary>
    ///   Manages the IPFS Directed Acrylic Graph.
    /// </summary>
    /// <remarks>
    ///   <note>
    ///   This is being obsoleted by <see cref="IDagApi"/>.
    ///   </note>
    /// </remarks>
    /// <seealso cref="IDagApi"/>
    /// <seealso cref="DagNode"/>
    /// <seealso href="https://github.com/ipfs/interface-ipfs-core/blob/master/SPEC/OBJECT.md">Object API spec</seealso>
    public interface IObjectApi
    {
        /// <summary>
        ///   Creates a new file directory in IPFS.
        /// </summary>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's value
        ///   is a <see cref="DagNode"/> to the new directory.
        /// </returns>
        /// <remarks>
        ///   Equivalent to <c>NewAsync("unixfs-dir")</c>.
        /// </remarks>
        Task<DagNode> NewDirectoryAsync(CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Create a new MerkleDAG node, using a specific layout.
        /// </summary>
        /// <param name="template"><b>null</b> or "unixfs-dir".</param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's value
        ///   is a <see cref="DagNode"/> to the new directory.
        /// </returns>
        /// <remarks>
        ///  Caveat: So far, only UnixFS object layouts are supported.
        /// </remarks>
        Task<DagNode> NewAsync(string template = null, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Fetch a MerkleDAG node.
        /// </summary>
        /// <param name="id">
        ///   The <see cref="Cid"/> to the node.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's value
        ///   is a <see cref="DagNode"/>.
        /// </returns>
        Task<DagNode> GetAsync(Cid id, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Store a MerkleDAG node.
        /// </summary>
        /// <param name="data">
        ///   The opaque data, can be <b>null</b>.
        /// </param>
        /// <param name="links">
        ///   The links to other nodes.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's value
        ///   is a <see cref="DagNode"/>.
        /// </returns>
        Task<DagNode> PutAsync(byte[] data, IEnumerable<IMerkleLink> links = null, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Store a MerkleDAG node.
        /// </summary>
        /// <param name="node">A merkle dag</param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's value
        ///   is a <see cref="DagNode"/>.
        /// </returns>
        Task<DagNode> PutAsync(DagNode node, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Get the data of a MerkleDAG node.
        /// </summary>
        /// <param name="id">
        ///   The <see cref="Cid"/> of the node.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's value
        ///   is a stream of data.
        /// </returns>
        /// <remarks>
        ///   The caller must dispose the returned <see cref="Stream"/>.
        /// </remarks>
        Task<Stream> DataAsync(Cid id, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Get the links of a MerkleDAG node.
        /// </summary>
        /// <param name="id">
        ///   The unique id of the node.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's value
        ///   is a sequence of links to the immediate children.
        /// </returns>
        /// <remarks>
        ///   <b>LinksAsync</b> returns the immediate child links of the <paramref name="id"/>.
        ///   To get all the children, this code can be used.
        ///   <code>
        ///   async Task&lt;List&lt;IMerkleLink>> AllLinksAsync(Cid cid)
        ///   {
        ///     var i = 0;
        ///     var allLinks = new List&lt;IMerkleLink>();
        ///     while (cid != null)
        ///     {
        ///        var links = await ipfs.Object.LinksAsync(cid);
        ///        allLinks.AddRange(links);
        ///        cid = (i &lt; allLinks.Count) ? allLinks[i++].Id : null;
        ///     }
        ///     return allLinks;
        ///   }
        ///   </code>
        /// </remarks>
        Task<IEnumerable<IMerkleLink>> LinksAsync(Cid id, CancellationToken cancel = default(CancellationToken));

    }
}
