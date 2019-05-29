# Core API

[ICoreApi](xref:Ipfs.CoreApi.ICoreApi) is a set of interfaces to IPFS features.


| Feature | Purpose |
| ------- | ------- |
| [Bitswap](xref:Ipfs.CoreApi.IBitswapApi) | Block trading between peers |
| [Block](xref:Ipfs.CoreApi.IBlockApi) | Manages the blocks |
| [BlockRepository](xref:Ipfs.CoreApi.IBlockRepositoryApi) | Manages the repository for [blocks](xref:Ipfs.CoreApi.IBlockApi) |
| [Bootstrap](xref:Ipfs.CoreApi.IBootstrapApi) | Trusted peers |
| [Config](xref:Ipfs.CoreApi.IConfigApi) | Manages the configuration of the local peer |
| [Dag](xref:Ipfs.CoreApi.IDagApi) | Manages the IPLD (linked data) Directed Acrylic Graph |
| [Dht](xref:Ipfs.CoreApi.IDhtApi) | Manages the Distributed Hash Table |
| [Dns](xref:Ipfs.CoreApi.IDnsApi) | DNS mapping to IPFS |
| [FileSystem](xref:Ipfs.CoreApi.IFileSystemApi) | Manages the files/directories in IPFS |
| [Key](xref:Ipfs.CoreApi.IKeyApi) | Manages the cryptographic keys |
| [Misc](xref:Ipfs.CoreApi.IGenericApi) | Some miscellaneous methods |
| [Name](xref:Ipfs.CoreApi.INameApi) | Manages the Interplanetary Name Space (IPNS) |
| [Object](xref:Ipfs.CoreApi.IObjectApi) | Manages the IPFS Directed Acrylic Graph |
| [Pin](xref:Ipfs.CoreApi.IPinApi) | Manage objects that are locally stored and permanent |
| [PubSub](xref:Ipfs.CoreApi.IPubSubApi) | Publish and subscribe topic messages |
| [Swarm](xref:Ipfs.CoreApi.ISwarmApi) | Manages the swarm of peers |
| [Stats](xref:Ipfs.CoreApi.IStatsApi) | Statistics on IPFS components |

