﻿namespace Data.Hash
{
    public interface IHashingHelper
    {
        void CreatePasswordHash(string password, out byte[] hash, out byte[] salt);

        bool VerifyPassword(string password, byte[] hash, byte[] salt);
    }
}
