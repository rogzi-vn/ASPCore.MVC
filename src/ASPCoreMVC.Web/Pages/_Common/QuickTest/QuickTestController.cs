using ASPCoreMVC.TCUEnglish.Vocabularies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace ASPCoreMVC.Web.Pages._Common.QuickTest
{
    [Authorize]
    public class QuickTestController : AbpController
    {
        private readonly IVocabularyService _VocabularyService;

        public QuickTestController(IVocabularyService _VocabularyService)
        {
            this._VocabularyService = _VocabularyService;
        }

        [HttpGet("/load-quick-test")]
        public async Task<IActionResult> LoadQuickTest()
        {
            var vocabs = await _VocabularyService.GenerateQuickVocabularyTests(5);
            return PartialView("~/Pages/_Common/QuickTest/QuickTestModal.cshtml", vocabs);
        }
    }
}
