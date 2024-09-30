# Bn254.Net

[![.NET Core CI](https://github.com/gldeng/Bn254.Net/actions/workflows/ci.yml/badge.svg)](https://github.com/gldeng/Bn254.Net/actions/workflows/ci.yml)

Bn254.Net is a .NET wrapper for the Rust library `Bn254`, which provides primitive operations on the BN254 (Barreto-Naehrig) elliptic curve. The underlying Rust library can be found [here](https://github.com/gldeng/libbn254).

## Features

- Point addition on G1
- Scalar multiplication
- Pairing checks

## Usage Examples

### Point Addition on G1

This operation corresponds to the Solidity precompile `0x06`.

```csharp
var x1 = new UInt256("18b18acfb4c2c30276db5411368e7185b311dd124691610c5d3b74034e093dc9");
var y1 = new UInt256("063c909c4720840cb5134cb9f59fa749755796819658d32efc0d288198f37266");
var x2 = new UInt256("07c2b7f58a84bd6145f00c9c2bc0bb1a187f20ff2c92963a88019e7c6a014eed");
var y2 = new UInt256("06614e20c147e940f2d70da3f74c9a17df361706a4485c742bd6788478fa17d7");
var x3e = new UInt256("2243525c5efd4b9c3d3c45ac0ca3fe4dd85e830a4ce6b65fa1eeaee202839703");
var y3e = new UInt256("301d1d33be6da8e509df21cc35964723180eed7532537db9ae5e7d48f195c915");

var (x3, y3) = Bn254.Add(x1, y1, x2, y2);
```
**Note: The `(x1, y1)` and `(x2, y2)` are G1 points say `P1` and `P2`. And `(x3, y3)` is the resultant point `P3` where `P3 = P1 + P2`.**

### Scalar Multiplication

This operation corresponds to the Solidity precompile `0x07`.

```csharp
var x1 = new UInt256("2bd3e6d0f3b142924f5ca7b49ce5b9d54c4703d7ae5648e61d02268b1a0a9fb7");
var y1 = new UInt256("21611ce0a6af85915e2f1d70300909ce2e49dfad4a4619c8390cae66cefdb204");
var s = new UInt256("00000000000000000000000000000000000000000000000011138ce750fa15c2");
var (x, y) = Bn254.Mul(x1, y1, s);
```

**Note: The `(x1, y1)` is the original point on G1 (say `P1`) and `s` is a value in the scalar field. And `(x, y)` represent a point `P` where `P = s*P1`.**

### Check Pairing

This operation corresponds to the Solidity precompile `0x08`.

```csharp
var one = (
    new UInt256("1c76476f4def4bb94541d57ebba1193381ffa7aa76ada664dd31c16024c43f59"),
    new UInt256("3034dd2920f673e204fee2811c678745fc819b55d3e9d294e45c9b03a76aef41"),
    new UInt256("209dd15ebff5d46c4bd888e51a93cf99a7329636c63514396b4a452003a35bf7"),
    new UInt256("04bf11ca01483bfa8b34b43561848d28905960114c8ac04049af4b6315a41678"),
    new UInt256("2bb8324af6cfc93537a2ad1a445cfd0ca2a71acd7ac41fadbf933c2a51be344d"),
    new UInt256("120a2a4cf30c1bf9845f20c6fe39e07ea2cce61f0c9bb048165fe5e4de877550")
);
var two = (
    new UInt256("111e129f1cf1097710d41c4ac70fcdfa5ba2023c6ff1cbeac322de49d1b6df7c"),
    new UInt256("2032c61a830e3c17286de9462bf242fca2883585b93870a73853face6a6bf411"),
    new UInt256("198e9393920d483a7260bfb731fb5d25f1aa493335a9e71297e485b7aef312c2"),
    new UInt256("1800deef121f1e76426a00665e5c4479674322d4f75edadd46debd5cd992f6ed"),
    new UInt256("090689d0585ff075ec9e99ad690c3395bc4b313370b38ef355acdadcd122975b"),
    new UInt256("12c85ea5db8c6deb4aab71808dcb408fe3d1e7690c43d37b4ce6cc0166fa7daa")
);
var valid = Bn254.Pairing(new[] { one, two });
```

**Note: The `Bn254.Pairing` method can check pairing of multiple pairs where each element in the input array is a tuple of 6 elements with the first two represents the G1 point and the last four represents the G2 point.**

