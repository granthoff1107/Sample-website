using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using FlowRepository.Data.Contracts;

namespace FlowRepository.Data.Rules
{
    //TODO: Make this non static and create and IHash interface so that we can decouple from implementation
    public class HashRule : IHash
    {
        public string CreateHash(string text, int workFactor = 12)
        {
            return BCrypt.Net.BCrypt.HashPassword(text, BCrypt.Net.BCrypt.GenerateSalt(workFactor));
        }

        public bool VerifyHash(string text, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(text, hash);
        }
    }
}
