// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.CompilerServices;
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

    public virtual bool VerifyHashedPassword(string hashedPassword, string providedPassword, out bool isRehashRequired) {
      if (hashedPassword == null) {
        throw new ArgumentNullException(nameof(hashedPassword));
      }
      if (providedPassword == null) {
        throw new ArgumentNullException(nameof(providedPassword));
      }

      isRehashRequired = false;
      byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);

      // read the format marker from the hashed password
      if (decodedHashedPassword.Length == 0) {
        return false;
      }
      switch (decodedHashedPassword[0]) {
        case 0x01:
          int embeddedIterCount;
          if (VerifyHashedPasswordV3(decodedHashedPassword, providedPassword, out embeddedIterCount)) {
            // If this hasher was configured with a higher iteration count, change the entry now.
            isRehashRequired = embeddedIterCount < _iterCount;
            return true;
          }
          return false;

        default:
          return false; // unknown format marker
      }
    }

    private static bool VerifyHashedPasswordV3(byte[] hashedPassword, string password, out int iterCount) {
      iterCount = default(int);

      try {
        // Read header information
        KeyDerivationPrf prf = (KeyDerivationPrf)ReadNetworkByteOrder(hashedPassword, 1);
        iterCount = (int)ReadNetworkByteOrder(hashedPassword, 5);
        int saltLength = (int)ReadNetworkByteOrder(hashedPassword, 9);

        // Read the salt: must be >= 128 bits
        if (saltLength < 128 / 8) {
          return false;
        }
        byte[] salt = new byte[saltLength];
        Buffer.BlockCopy(hashedPassword, 13, salt, 0, salt.Length);

        // Read the subkey (the rest of the payload): must be >= 128 bits
        int subkeyLength = hashedPassword.Length - 13 - salt.Length;
        if (subkeyLength < 128 / 8) {
          return false;
        }
        byte[] expectedSubkey = new byte[subkeyLength];
        Buffer.BlockCopy(hashedPassword, 13 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

        // Hash the incoming password and verify it
        byte[] actualSubkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, subkeyLength);
        return ByteArraysEqual(actualSubkey, expectedSubkey);
      } catch {
        // This should never occur except in the case of a malformed payload, where
        // we might go off the end of the array. Regardless, a malformed payload
        // implies verification failed.
        return false;
      }
    }

    // Compares two byte arrays for equality. The method is specifically written so that the loop is not optimized.
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    private static bool ByteArraysEqual(byte[] a, byte[] b) {
      if (a == null && b == null) {
        return true;
      }
      if (a == null || b == null || a.Length != b.Length) {
        return false;
      }
      var areSame = true;
      for (var i = 0; i < a.Length; i++) {
        areSame &= (a[i] == b[i]);
      }
      return areSame;
    }

    private static uint ReadNetworkByteOrder(byte[] buffer, int offset) {
      return ((uint)(buffer[offset + 0]) << 24)
          | ((uint)(buffer[offset + 1]) << 16)
          | ((uint)(buffer[offset + 2]) << 8)
          | buffer[offset + 3];
    }
  }
}
