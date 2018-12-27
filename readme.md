RandomImpls
=======

Provides a few useful overrides of the System.Random class.

[![MIT Licensed](https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square)](license.md)
[![Get it on NuGet](https://img.shields.io/nuget/v/RandomImpls.svg?style=flat-square)](http://nuget.org/packages/RandomImpls)

[![Appveyor Build](https://img.shields.io/appveyor/ci/otac0n/RandomImpls.svg?style=flat-square)](https://ci.appveyor.com/project/otac0n/RandomImpls)
[![Test Coverage](https://img.shields.io/codecov/c/github/otac0n/RandomImpls.svg?style=flat-square)](https://codecov.io/gh/otac0n/RandomImpls)
[![Pre-release packages available](https://img.shields.io/nuget/vpre/RandomImpls.svg?style=flat-square)](http://nuget.org/packages/RandomImpls)

[Dice icon by Petai Jantrapoon from the Noun Project](https://thenounproject.com/term/dice/1540257/)

Getting Started
---------------

    PM> Install-Package RandomImpls

Available Implementations
-------------------------

* `HashAlgorithmRandom`  
    Wraps a `System.Security.Cryptography.HashAlgorithm` to provide repeatable randomness based on a seed of arbitrary bytes.  
    Example:
    ```
    using System.Security.Cryptography;
    
    byte[] seed = null; // Provide a seed here.
    HashAlgorithm hash = new SHA256Managed();
    Random rand = new HashAlgorithmRandom(hash, seed);
    ```
* `RandomNumberGeneratorRandom`  
    Wraps a `System.Security.Cryptography.RandomNumberGenerator` to provide cryptographically secure randomness in an interface that allows interoperating
    with libraries that use `System.Random`.  
    Example:
    ```
    using System.Security.Cryptography;
    
    RandomNumberGenerator  rng = new RNGCryptoServiceProvider();
    Random rand = new RandomNumberGeneratorRandom(rng);
    ```
* `StaticRandom`  
    Provides static versions of the methods in the `System.Random` class.  Internally this class uses a `[ThreadStatic]` field to segregate threads, and
    chooses seed values that are not likely to collide when multiple threads are started at the same time.  
    Example:
    ```
    int value = StaticRandom.Next();
    ```
* `BytesRandom`  
    A base class that produces randomness based on an implementation-specific stream of bytes.  See the implementations of `HashAlgorithmRandom` or
    `RandomNumberGeneratorRandom` for implementation examples.
