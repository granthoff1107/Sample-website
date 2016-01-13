﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.ExendedModels.Contracts
{
    public interface ITagRepository : IRepository
    {
        List<Tag> TagsStartingWith(string term);        
    }
}