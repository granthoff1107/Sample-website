using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.Repositories.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Repositories.Models.FlowRepository
{
    public class TagRepository : DataRepository<FlowCollectionEntities>, ITagRepository
    {
        public TagRepository() : base()
        {
        }

        public TagRepository(FlowCollectionEntities context)
            : base(context)
        {
        }

        public List<Tag> TagsStartingWith(string term)
        {
            return All<Tag>().Where(t => t.Name.ToLower().StartsWith(term)).ToList();
        }
    }
}
