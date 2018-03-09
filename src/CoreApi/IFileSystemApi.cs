using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ipfs.CoreApi
{
    /// <summary>
    ///   Manages the files/directories in IPFS.
    /// </summary>
    /// <seealso href="https://github.com/ipfs/interface-ipfs-core/blob/master/SPEC/FILES.md">Files API spec</seealso>
    public interface IFileSystemApi
    {
        /// <summary>
        ///   Add a local file to the interplanetary file system.
        /// </summary>
        /// <param name="path">
        ///   The name of the local file.
        /// </param>
        /// <param name="pin">
        ///   If <b>true</b> the file is pinned to local storage and will not be
        ///   garbage collected.  The default is <b>true</b>.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///    A task that represents the asynchronous operation. The task's value is
        ///    the file's node.
        /// </returns>
        Task<IFileSystemNode> AddFileAsync(string path, bool pin = true, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Add some text to the interplanetary file system.
        /// </summary>
        /// <param name="text">
        ///   The string to add to IPFS.  It is UTF-8 encoded.
        /// </param>
        /// <param name="pin">
        ///   If <b>true</b> the text is pinned to local storage and will not be
        ///   garbage collected.  The default is <b>false</b>.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's value is
        ///   the text's node.
        /// </returns>
        Task<IFileSystemNode> AddTextAsync(string text, bool pin = true, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Add a <see cref="Stream"/> to interplanetary file system.
        /// </summary>
        /// <param name="stream">
        ///   The stream of data to add to IPFS.
        /// </param>
        /// <param name="name">
        ///   A name for the <paramref name="stream"/>.
        /// </param>
        /// <param name="pin">
        ///   If <b>true</b> the stream's data is pinned to local storage and will not be
        ///   garbage collected.  The default is <b>false</b>.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's value is
        ///   the data's node.
        /// </returns>
        Task<IFileSystemNode> AddAsync(Stream stream, string name = "", bool pin = true, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Add a directory and its files to the interplanetary file system.
        /// </summary>
        /// <param name="path">
        ///   The path to directory.
        /// </param>
        /// <param name="recursive">
        ///   <b>true</b> to add sub-folders.
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's value is
        ///   the directory's node.
        /// </returns>
        Task<IFileSystemNode> AddDirectoryAsync(string path, bool recursive = true, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Reads the content of an existing IPFS file as text.
        /// </summary>
        /// <param name="path">
        ///   A path to an existing file, such as "QmXarR6rgkQ2fDSHjSY5nM2kuCXKYGViky5nohtwgF65Ec/about"
        ///   or "QmZTR5bcpQD7cFgTorqxZDYaew1Wqgfbd2ud9QqGPAkK2V"
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's value is
        ///   the contents of the <paramref name="path"/> as a <see cref="string"/>.
        /// </returns>
        Task<String> ReadAllTextAsync(string path, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Reads an existing IPFS file.
        /// </summary>
        /// <param name="path">
        ///   A path to an existing file, such as "QmXarR6rgkQ2fDSHjSY5nM2kuCXKYGViky5nohtwgF65Ec/about"
        ///   or "QmZTR5bcpQD7cFgTorqxZDYaew1Wqgfbd2ud9QqGPAkK2V"
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns>
        ///   A task that represents the asynchronous operation. The task's value is
        ///   a <see cref="Stream"/> to the file contents.
        /// </returns>
        Task<Stream> ReadFileAsync(string path, CancellationToken cancel = default(CancellationToken));

        /// <summary>
        ///   Get information about the file or directory.
        /// </summary>
        /// <param name="path">
        ///   A path to an existing file or directory, such as "QmXarR6rgkQ2fDSHjSY5nM2kuCXKYGViky5nohtwgF65Ec/about"
        ///   or "QmZTR5bcpQD7cFgTorqxZDYaew1Wqgfbd2ud9QqGPAkK2V"
        /// </param>
        /// <param name="cancel">
        ///   Is used to stop the task.  When cancelled, the <see cref="TaskCanceledException"/> is raised.
        /// </param>
        /// <returns></returns>
        Task<IFileSystemNode> ListFileAsync(string path, CancellationToken cancel = default(CancellationToken));
    }
}
