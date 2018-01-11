using System;
using System.Collections.Generic;
using System.Text;

namespace Ipfs
{
    /// <summary>
    ///   InterPlanetary Linked Data.
    /// </summary>
    /// <remarks>
    ///   <note>Not yet ready for prime time.</note>
    /// </remarks>
    /// <seealso href="https://github.com/ipld"/>
    public interface ILinkedNode : IMerkleNode<IMerkleLink>
    {
    }
}
