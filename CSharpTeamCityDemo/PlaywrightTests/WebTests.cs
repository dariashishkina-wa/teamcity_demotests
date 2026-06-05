using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace PlaywrightTests;

[TestFixture]
public class WebTests : PageTest
{
    [Test]
    public async Task PlaywrightDev_ShouldHaveTitle()
    {
        await Page.GotoAsync("https://playwright.dev/dotnet/");

        await Expect(Page).ToHaveTitleAsync(new System.Text.RegularExpressions.Regex("Playwright"));
    }

    [Test]
    public async Task PlaywrightDev_GetStartedLink_ShouldNavigate()
    {
        await Page.GotoAsync("https://playwright.dev/dotnet/");

        await Page.GetByRole(Microsoft.Playwright.AriaRole.Link, new() { Name = "Get started" }).ClickAsync();

        await Expect(Page).ToHaveURLAsync(new System.Text.RegularExpressions.Regex(".*intro"));
    }
}
