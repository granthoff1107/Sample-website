using Flowbandit.Models.Generic;
using FlowRepository.Repositories.Contracts.FlowRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flowbandit.Models.Search
{
    public class SearchResultsVM : BaseModel<IVideoRepository>
    {
        public SearchResultsVM(IVideoRepository repository, string searchQuery, string repo) : base(repository)
        {
            this.GenerateResults(repository, searchQuery, repo);
        }

        protected void GenerateResults(IVideoRepository repository, string searchQuery, string repo)
        {
            string[] terms = searchQuery.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var term in terms)
            {
                
            }

        }
    }
}