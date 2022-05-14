namespace Geronimus.Text.Tests;

[TestClass]
public class WindowLineEndingsTests
{
    [TestMethod]
    public void GivenNull_ReturnsEmptyString()
    {
        Assert.AreEqual(
            string.Empty,
        #nullable disable
            Characters.WindowsLineEndings( null ),
        #nullable restore
            false
        );
    }

    [TestMethod]
    public void GivenMixedEndings_ReturnsOnlyWindowsStyle()
    {
        const string mixed =
            "Line 1\rLine 2\r\nLine 3\nLine4\u2028Line5\r\n\n";
        const string expected =
            "Line 1\r\nLine 2\r\nLine 3\r\nLine4\r\nLine5\r\n\r\n";

        Assert.AreEqual(
            expected,
            Characters.WindowsLineEndings( mixed ),
            false
        );
    }
}