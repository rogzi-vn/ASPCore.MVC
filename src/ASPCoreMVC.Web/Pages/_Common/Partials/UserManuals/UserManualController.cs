using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace ASPCoreMVC.Web.Pages._Common.Partials.UserManuals
{
    [Authorize]
    [Route("/ui/helpers/user-manuals")]
    public class UserManualController : AbpController
    {
        [HttpGet]
        [Route("skill-part-instructions-prop")]
        public IActionResult SkillPartInstructionsProp()
        {
            return PartialView($"~/Pages/_Common/Partials/UserManuals/{nameof(SkillPartInstructionsProp)}.cshtml");
        }
        [HttpGet]
        [Route("skill-part-editor-layout-prop")]
        public IActionResult SkillPartEditorLayoutProp()
        {
            return PartialView($"~/Pages/_Common/Partials/UserManuals/{nameof(SkillPartEditorLayoutProp)}.cshtml");
        }
        [HttpGet]
        [Route("skill-part-display-question-text-prop")]
        public IActionResult SkillIsDisplayQuestionTextProp()
        {
            return PartialView($"~/Pages/_Common/Partials/UserManuals/{nameof(SkillIsDisplayQuestionTextProp)}.cshtml");
        }
    }
}
