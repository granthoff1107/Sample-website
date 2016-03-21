using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Tests.General
{
    public class TestLoremGenerator
    {
        #region Members
        
        //I don't care if it not fully proper, GetHashCode should be good enough for a random seed
        protected static Random random = new Random(Guid.NewGuid().GetHashCode());
        protected string _ipsumLorem; 

        #endregion

        #region Constructors

        public TestLoremGenerator(string fileLocation)
        {
            _ipsumLorem = File.ReadAllText(fileLocation);
        } 

        #endregion

        #region Public Methods

        public string GetLorem(int textLength)
        {
            var textLimit = _ipsumLorem.Length;
            var startIndex = random.Next(textLimit - textLength);
            return _ipsumLorem.Substring(startIndex, textLength);
        } 

        #endregion
    }
}
