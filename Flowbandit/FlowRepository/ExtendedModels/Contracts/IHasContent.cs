using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.ExtendedModels.Contracts
{
    public interface IHasContent
    {
        int Id { get; set; }
        Content Content { get; set; }
    }
}
