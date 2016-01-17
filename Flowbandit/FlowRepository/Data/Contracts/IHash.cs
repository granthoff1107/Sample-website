using System;
namespace FlowRepository.Data.Contracts
{
    public interface IHash
    {
        string CreateHash(string text, int workFactor = 12);
        bool VerifyHash(string text, string hash);
    }
}
