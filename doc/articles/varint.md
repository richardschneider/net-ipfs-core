# Variable Integer

A [varint](xref:Ipfs.Varint) is used to encode a non-negative integer of up to 64 bits. 
It is encoded in network byte order (Big Endian). Each byte (except the last) contains 7 bits
of information with the most significant bit set to 1.  The last byte has the MSB set to 0.

| Value | Varint encoding |
| ----- | --------------- |
| 1 (0x1)              | 01 |
| 16 (0x10)            | 10 |
| 256 (0x100)          | 80 02 |
| 4096 (0x1000)        | 80 20 |
| 65536 (0x10000)      | 80 80 04 |
| 1048576 (0x100000)   | 80 80 40 |
| 16777216 (0x1000000) | 80 80 80 08 |
