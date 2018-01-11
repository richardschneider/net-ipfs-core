using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipfs.CoreApi
{
    /// <summary>
    ///   Manage pinned objects (locally stored and permanent).
    /// </summary>
    /// <seealso href="https://github.com/ipfs/interface-ipfs-core/blob/master/SPEC/PIN.md">Pin API spec</seealso>
    public interface IPinApi
    {
        /// <summary>
        ///   Adds an IPFS object to the pinset and also stores it to the IPFS repo. pinset is the set of hashes currently pinned (not gc'able).
        /// </summary>
        /// <param name="path">
        ///   A CID or path to an existing object, such as "QmXarR6rgkQ2fDSHjSY5nM2kuCXKYGViky5nohtwgF65Ec/about"
        ///   or "QmZTR5bcpQD7cFgTorqxZDYaew1Wqgfbd2ud9QqGPAkK2V"
        /// </param>
        /// <param name="recursive">
        ///   <b>true</b> to recursively pin links of the object; otherwise, <b>false</b> to only pin
        ///   the specified object.  Default is <b>true</b>.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's value
        ///   is a sequence of <see cref="Cid"/> that were pinned.
        /// </returns>
        Task<IEnumerable<Cid>> AddAsync(string path, bool recursive = true, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   List all the objects pinned to local storage.
        /// </summary>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's value
        ///   is a sequence of <see cref="Cid"/>.
        /// </returns>
        Task<IEnumerable<Cid>> ListAsync(CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Unpin an object.
        /// </summary>
        /// <param name="id">
        ///   The CID of the object.
        /// </param>
        /// <param name="recursive">
        ///   <b>true</b> to recursively unpin links of object; otherwise, <b>false</b> to only unpin
        ///   the specified object.  Default is <b>true</b>.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's value
        ///   is a sequence of <see cref="Cid"/> that were unpinned.
        /// </returns>
        Task<IEnumerable<Cid>> RemoveAsync(Cid id, bool recursive = true, CancellationToken cancel = default(CancellationToken));

    }
}
