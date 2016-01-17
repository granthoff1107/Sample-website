using FlowRepository;
using FlowRepository.Repositories.Contracts.FlowRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flowbandit.Controllers.Rules
{
    public class TagsRule
    {
        public static List<AutoCompleteResult> GetAutocompleteResults(ITagRepository repository, string term)
        {
            return repository.TagsStartingWith(term).Select(t => new AutoCompleteResult { label = t.Name, value = t.ID.ToString() }).ToList();
        }
    }
}