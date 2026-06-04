using NUnit.Framework;

namespace DemoTests;

[TestFixture]
public class CalculatorTests
{
    [Test]
    public void Add_TwoNumbers_ReturnsSum()
    {
        Assert.That(2 + 3, Is.EqualTo(5));
    }

    [Test]
    public void Subtract_TwoNumbers_ReturnsDifference()
    {
        Assert.That(10 - 4, Is.EqualTo(6));
    }

    [TestCase(1, 1, 2)]
    [TestCase(0, 0, 0)]
    [TestCase(-1, 1, 0)]
    public void Add_Parameterized(int a, int b, int expected)
    {
        Assert.That(a + b, Is.EqualTo(expected));
    }
}
