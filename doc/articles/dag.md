# Merkle DAG

The [DagNode](xref:Ipfs.DagNode) is a directed acyclic graph whose edges are a 
[DagLink](xref:Ipfs.DagLink). This means that links to objects can authenticate 
the objects themselves, and that every object contains a secure 
representation of its children.

Every Merkle node is a directed acyclic graph (DAG) because each child node is accessed via its [CID](cid.md), 
basically the [hash](multihash.md) of the child's dag node. It's [ID](xref:Ipfs.DagNode.Id) 
is the hash of its local content ([data](xref:Ipfs.DagNode.DataBytes) and [links](xref:Ipfs.DagNode.Links)). 
So after creation there is no way to edit a DagNode. This prevents cycles (assuming there are no hash collisions) 
since one can not link the first created node to the last node to create the last reference.

```csharp
var a = Encoding.UTF8.GetBytes("a");
var anode = new DagNode(a);
var alink = anode.ToLink("a");

var b = Encoding.UTF8.GetBytes("b");
var bnode = new DagNode(b);
var blink = bnode.ToLink("b");

var node = new DagNode(null, new[] { alink, blink });
Assert.AreEqual(2, node.Links.Count());
Assert.AreEqual("QmbNgNPPykP4YTuAeSa3DsnBJWLVxccrqLUZDPNQfizGKs", (string)node.Id);
```
