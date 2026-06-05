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
        // Create screenshots directory for each test
        _screenshotsDir = Path.Combine(
            TestContext.CurrentContext.WorkDirectory, 
            ""test-artifacts"",
            TestContext.CurrentContext.Test.Name);
        Directory.CreateDirectory(_screenshotsDir);

        // Start tracing for each test (captures actions, screenshots, DOM snapshots)
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
        // Save trace file after each test
        var tracePath = Path.Combine(_screenshotsDir, ""trace.zip"");
        await Page.Context.Tracing.StopAsync(new() { Path = tracePath });

        // Take final screenshot on failure
        if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            await Page.ScreenshotAsync(new()
            {
                Path = Path.Combine(_screenshotsDir, ""failure.png""),
                FullPage = true
            });
        }
    }

    [Test]
    public async Task PlaywrightDev_ShouldHaveCorrectTitle()
    {
        // Step 1: Navigate to Playwright docs
        await Page.GotoAsync(""https://playwright.dev/dotnet/"");
        await Page.ScreenshotAsync(new()
        {
            Path = Path.Combine(_screenshotsDir, ""01_homepage.png"")
        });

        // Step 2: Verify title
        await Expect(Page).ToHaveTitleAsync(new System.Text.RegularExpressions.Regex(""Playwright""));

        // Step 3: Verify hero text is visible
        var heading = Page.GetByRole(AriaRole.Heading, new() { Name = ""Playwright"" }).First;
        await Expect(heading).ToBeVisibleAsync();
        await Page.ScreenshotAsync(new()
        {
            Path = Path.Combine(_screenshotsDir, ""02_heading_visible.png"")
        });
    }

    [Test]
    public async Task PlaywrightDev_NavigateToGetStarted()
    {
        // Step 1: Open homepage
        await Page.GotoAsync(""https://playwright.dev/dotnet/"");
        await Page.ScreenshotAsync(new()
        {
            Path = Path.Combine(_screenshotsDir, ""01_homepage.png"")
        });

        // Step 2: Click Get Started
        await Page.GetByRole(AriaRole.Link, new() { Name = ""Get started"" }).ClickAsync();
        await Page.ScreenshotAsync(new()
        {
            Path = Path.Combine(_screenshotsDir, ""02_after_click.png"")
        });

        // Step 3: Verify navigation
        await Expect(Page).ToHaveURLAsync(new System.Text.RegularExpressions.Regex("".*intro""));

        // Step 4: Verify Installation heading exists
        var installHeading = Page.GetByRole(AriaRole.Heading, new() { Name = ""Installation"" });
        await Expect(installHeading).ToBeVisibleAsync();
        await Page.ScreenshotAsync(new()
        {
            Path = Path.Combine(_screenshotsDir, ""03_installation_section.png"")
        });
    }

    [Test]
    public async Task PlaywrightDev_SearchFunctionality()
    {
        // Step 1: Open homepage
        await Page.GotoAsync(""https://playwright.dev/dotnet/"");
        await Page.ScreenshotAsync(new()
        {
            Path = Path.Combine(_screenshotsDir, ""01_homepage.png"")
        });

        // Step 2: Click search button
        var searchButton = Page.GetByRole(AriaRole.Button, new() { Name = ""Search"" });
        await searchButton.ClickAsync();
        await Page.ScreenshotAsync(new()
        {
            Path = Path.Combine(_screenshotsDir, ""02_search_opened.png"")
        });

        // Step 3: Type search query
        var searchInput = Page.GetByPlaceholder(""Search docs"");
        await searchInput.FillAsync(""locator"");
        await Page.ScreenshotAsync(new()
        {
            Path = Path.Combine(_screenshotsDir, ""03_search_results.png"")
        });

        // Step 4: Verify search results appear
        var results = Page.Locator("".DocSearch-Hits, [class*='searchResultsColumn']"").First;
        await Expect(results).ToBeVisibleAsync(new() { Timeout = 5000 });
    }
}
