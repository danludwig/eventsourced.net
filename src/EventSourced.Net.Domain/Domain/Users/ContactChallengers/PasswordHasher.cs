// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography;
using Microsoft.AspNet.Cryptography.KeyDerivation;

namespace EventSourced.Net.Domain.Users.ContactChallengers
{
  internal class PasswordHasher
  {
    internal static readonly PasswordHasher Instance = new PasswordHasher();
    private readonly int _iterCount;
    private readonly RandomNumberGenerator _rng;

    private PasswordHasher() {
      _iterCount = 10000;
      if (_iterCount < 1) {
        throw new InvalidOperationException("InvalidPasswordHasherIterationCount");
      }
      _rng = RandomNumberGenerator.Create();
    }

    internal string HashPassword(string password) {
      if (password == null) throw new ArgumentNullException(nameof(password));
      return Convert.ToBase64String(HashPasswordV3(password, _rng));
    }

    private byte[] HashPasswordV3(string password, RandomNumberGenerator rng) {
      return HashPasswordV3(password, rng,
        prf: KeyDerivationPrf.HMACSHA256,
        iterCount: _iterCount,
        saltSize: 128 / 8,
        numBytesRequested: 256 / 8);
    }

    private static byte[] HashPasswordV3(string password, RandomNumberGenerator rng, KeyDerivationPrf prf, int iterCount, int saltSize, int numBytesRequested) {
      // Produce a version 3 (see comment above) text hash.
      byte[] salt = new byte[saltSize];
      rng.GetBytes(salt);
      byte[] subkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, numBytesRequested);

      var outputBytes = new byte[13 + salt.Length + subkey.Length];
      outputBytes[0] = 0x01; // format marker
      WriteNetworkByteOrder(outputBytes, 1, (uint)prf);
      WriteNetworkByteOrder(outputBytes, 5, (uint)iterCount);
      WriteNetworkByteOrder(outputBytes, 9, (uint)saltSize);
      Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
      Buffer.BlockCopy(subkey, 0, outputBytes, 13 + saltSize, subkey.Length);
      return outputBytes;
    }

    private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value) {
      buffer[offset + 0] = (byte)(value >> 24);
      buffer[offset + 1] = (byte)(value >> 16);
      buffer[offset + 2] = (byte)(value >> 8);
      buffer[offset + 3] = (byte)(value >> 0);
    }
  }
}
