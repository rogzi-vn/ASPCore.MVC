using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace ASPCoreMVC.Pages
{
    public class Index_Tests : ASPCoreMVCWebTestBase
    {
        [Fact]
        public async Task Welcome_Page()
        {
            var response = await GetResponseAsStringAsync("/");
            response.ShouldNotBeNull();
        }
    }
}
