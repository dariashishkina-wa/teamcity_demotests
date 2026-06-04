using NUnit.Framework;

namespace DemoTests;

[TestFixture]
public class CalculatorTests
{
    [Test]
    [Category("Smoke")]
    public void Add_TwoNumbers_ReturnsSum()
    {
        Assert.That(2 + 3, Is.EqualTo(99));  // Специально сломано! 5 != 99
    }

    [Test]
    [Category("Smoke")]
    public void Subtract_TwoNumbers_ReturnsDifference()
    {
        Assert.That(10 - 4, Is.EqualTo(6));
    }

    [TestCase(1, 1, 2)]
    [TestCase(0, 0, 0)]
    [TestCase(-1, 1, 0)]
    [Category("Regression")]
    public void Add_Parameterized(int a, int b, int expected)
    {
        Assert.That(a + b, Is.EqualTo(expected));
    }
}
