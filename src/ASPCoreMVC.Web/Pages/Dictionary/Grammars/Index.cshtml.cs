using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCoreMVC.TCUEnglish.GrammarCategories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPCoreMVC.Web.Pages.Dictionary.Grammars
{
    public class DicGramIndexModel : AppPageModel
    {
        private readonly IGrammarCategoryService _GrammarCategoryService;
        public DicGramIndexModel(IGrammarCategoryService _GrammarCategoryService)
        {
            this._GrammarCategoryService = _GrammarCategoryService;
        }
        public async Task OnGetAsync()
        {
            ViewData["GrammarCategories"] = (await _GrammarCategoryService.GetBase()).Data;
        }
    }
}
