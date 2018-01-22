# net-ipfs-core 

[![build status](https://ci.appveyor.com/api/projects/status/github/richardschneider/net-ipfs-core?branch=master&svg=true)](https://ci.appveyor.com/project/richardschneider/net-ipfs-core) 
[![travis build](https://travis-ci.org/richardschneider/net-ipfs-core.svg?branch=master)](https://travis-ci.org/richardschneider/net-ipfs-core)
[![Coverage Status](https://coveralls.io/repos/richardschneider/net-ipfs-core/badge.svg?branch=master&service=github)](https://coveralls.io/github/richardschneider/net-ipfs-core?branch=master)
[![Version](https://img.shields.io/nuget/v/Ipfs.Core.svg)](https://www.nuget.org/packages/Ipfs.Core)
[![docs](https://cdn.rawgit.com/richardschneider/net-ipfs-core/master/doc/images/docs-latest-green.svg)](https://richardschneider.github.io/net-ipfs-core)

The core objects and interfaces of the [IPFS](https://github.com/ipfs/ipfs) (Inter Planetary File System) for .Net (C#, VB, F# etc.)

The interplanetary file system is the permanent web. It is a new hypermedia distribution protocol, addressed by content and identities. IPFS enables the creation of completely distributed applications. It aims to make the web faster, safer, and more open.

It supports the following runtimes

- .NET Framework 4.5
- .NET Standard 1.4
- .NET Standard 2.0

More information is on the [Documentation](https://richardschneider.github.io/net-ipfs-core/) web site.

## Getting started

Published releases of IPFS Core are available on [NuGet](https://www.nuget.org/packages/ipfs.core/).  To install, run the following command in the [Package Manager Console](https://docs.nuget.org/docs/start-here/using-the-package-manager-console).

    PM> Install-Package Ipfs.Core
    
For the latest build or older non-released builds see [Continuous Integration](https://github.com/richardschneider/net-ipfs-core/wiki/Continuous-Integration).

## Major objects

- [MerkleDag](https://richardschneider.github.io/net-ipfs-core/api/Ipfs.DagNode.html)
- [MultiAddress](https://richardschneider.github.io/net-ipfs-core/api/Ipfs.MultiAddress.html)
- [MultiHash](https://richardschneider.github.io/net-ipfs-core/api/Ipfs.MultiHash.html)

See the [API Documentation](https://richardschneider.github.io/net-ipfs-core/api/Ipfs.html) for a list of all objects.

### MultiHash

All hashes in IPFS are encoded with [multihash](https://github.com/multiformats/multihash), a self-describing hash format. The actual hash function used depends on security requirements. The cryptosystem of IPFS is upgradeable, meaning that as hash functions are broken, networks can shift to stronger hashes. There is no free lunch, as objects may need to be rehashed, or links duplicated. But ensuring that tools built do not assume a pre-defined length of hash digest means tools that work with today's hash functions will also work with tomorrows longer hash functions too.

### MultiAddress

A standard way to represent a networks address that supports [multiple network protocols](https://github.com/multiformats/multiaddr). It is represented as a series of tuples, a protocol code and an optional value.  For example, an IPFS file at a sepcific address over ipv4 and tcp is 

    /ip4/10.1.10.10/tcp/80/ipfs/QmVcSqVEsvm5RR9mBLjwpb2XjFVn5bPdPL69mL8PH45pPC

### Merkle DAG

The [DagNode](https://richardschneider.github.io/net-ipfs-core/api/Ipfs.DagNode.html) is a directed acyclic graph whose edges are a 
[DagLink](https://richardschneider.github.io/net-ipfs-core/api/Ipfs.DagLink.html). This means that links to objects can authenticate 
the objects themselves, and that every object contains a secure 
representation of its children.

Every Merkle is a directed acyclic graph (DAG) because each node is accessed via its name (the hash of `DagNode`). Each branch of Merkle is the hash of its local content (data and links);  naming children by their hash instead of their full contents. So after creation there is no way to edit a DagNode. This prevents cycles (assuming there are no hash collisions) since one can not link the first created node to the last note to create the last reference.

## Base58

Most binary data (objects) in IPFS is represented as a [Base-58](https://en.wikipedia.org/wiki/Base58) string; the BitCoin alphabet is used.

> Base58 is a group of binary-to-text encoding schemes used to represent large integers as alphanumeric text. It is similar to Base64 but has been modified to avoid both non-alphanumeric characters and letters which might look ambiguous when printed. It is therefore designed for human users who manually enter the data, copying from some visual source, but also allows easy copy and paste because a double-click will usually select the whole string. 

# License
Copyright © 2015-2018 Richard Schneider (makaretu@gmail.com)

The IPFS Core library is licensed under the [MIT](http://www.opensource.org/licenses/mit-license.php "Read more about the MIT license form") license. Refere to the [LICENSE](https://github.com/richardschneider/net-ipfs-core/blob/master/LICENSE) file for more information.
