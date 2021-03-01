using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCoreMVC.TCUEnglish.Vocabularies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPCoreMVC.Web.Pages.Dictionary.Vocabularies
{
    public class DicVocaIndexModel : AppPageModel
    {
        private readonly IVocabularyService _VocabularyService;

        public List<VocabularySimpifyDTO> Vocabs { get; set; }
        public DicVocaIndexModel(IVocabularyService _VocabularyService)
        {
            this._VocabularyService = _VocabularyService;
        }
        public async Task OnGetAsync()
        {
            var res = await _VocabularyService.GetRandomVocabularies(30);
            if (res.Success && res.Data != null)
            {
                Vocabs = res.Data;
            }
        }
    }
}
