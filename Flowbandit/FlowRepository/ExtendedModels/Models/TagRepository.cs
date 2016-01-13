using FlowRepository.ExendedModels.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.ExendedModels.Models
{
    public class TagRepository : DataRepository, ITagRepository
    {
        public List<Tag> TagsStartingWith(string term)
        {
            return All<Tag>().Where(t => t.Name.ToLower().StartsWith(term)).ToList();
        }
    }
}
