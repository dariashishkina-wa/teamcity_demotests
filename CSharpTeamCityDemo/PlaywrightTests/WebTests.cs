using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace PlaywrightTests;

[TestFixture]
public class WebTests : PageTest
{
    private string _screenshotsDir = null!;

    [SetUp]
    public void SetUp()
    {
        _screenshotsDir = Path.Combine(
            TestContext.CurrentContext.WorkDirectory,
            "test-artifacts",
            TestContext.CurrentContext.Test.Name);
        Directory.CreateDirectory(_screenshotsDir);

        Page.Context.Tracing.StartAsync(new()
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        }).GetAwaiter().GetResult();
    }

    [TearDown]
    public async Task TearDown()
    {
        var tracePath = Path.Combine(_screenshotsDir, "trace.zip");
        await Page.Context.Tracing.StopAsync(new() { Path = tracePath });

        if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            await Page.ScreenshotAsync(new()
            {
                Path = Path.Combine(_screenshotsDir, "failure.png"),
                FullPage = true
            });
        }
    }

    [Test]
    public async Task PlaywrightDev_ShouldHaveCorrectTitle()
    {
        await Page.GotoAsync("https://playwright.dev/dotnet/");
        await Page.ScreenshotAsync(new()
        {
            Path = Path.Combine(_screenshotsDir, "01_homepage.png")
        });

        await Expect(Page).ToHaveTitleAsync(new System.Text.RegularExpressions.Regex("Playwright"));

        var heading = Page.GetByRole(AriaRole.Heading, new() { Name = "Playwright" }).First;
        await Expect(heading).ToBeVisibleAsync();
        await Page.ScreenshotAsync(new()
        {
            Path = Path.Combine(_screenshotsDir, "02_heading_visible.png")
        });
    }

    [Test]
    public async Task PlaywrightDev_NavigateToGetStarted()
    {
        await Page.GotoAsync("https://playwright.dev/dotnet/");
        await Page.ScreenshotAsync(new()
        {
            Path = Path.Combine(_screenshotsDir, "01_homepage.png")
        });

        await Page.GetByRole(AriaRole.Link, new() { Name = "Get started" }).ClickAsync();
        await Page.ScreenshotAsync(new()
        {
            Path = Path.Combine(_screenshotsDir, "02_after_click.png")
        });

        await Expect(Page).ToHaveURLAsync(new System.Text.RegularExpressions.Regex(".*intro"));

        var installHeading = Page.GetByRole(AriaRole.Heading, new() { Name = "Installation" });
        await Expect(installHeading).ToBeVisibleAsync();
        await Page.ScreenshotAsync(new()
        {
            Path = Path.Combine(_screenshotsDir, "03_installation_section.png")
        });
    }
}
