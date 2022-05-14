namespace Geronimus.Text.Tests;

[TestClass]
public class NormalizeLineEndingsTests
{
    [TestMethod]
    public void GivenNull_ReturnsEmptyString()
    {
        Assert.AreEqual(
            string.Empty,
        #nullable disable
            Characters.NormalizeLineEndings( null ),
        #nullable restore
            false
        );
    }

    [TestMethod]
    public void GivenNoLineEnd_ReturnsTheSameString()
    {
        const string example = "A single line of text...";

        Assert.AreEqual(
            example,
            Characters.NormalizeLineEndings( example ),
            false
        );
    }

    [TestMethod]
    public void GivenOnlyLineFeeds_ReturnsTheSameString()
    {
        const string example = "Line 1\nLine 2\nLine 3";

        Assert.AreEqual(
            example,
            Characters.NormalizeLineEndings( example ),
            false
        );
    }

    [TestMethod]
    public void GivenWindowsLineEndings_ReturnsOnlyLineFeeds()
    {
        const string windowsStyle = "Line 1\r\nLine 2\r\nLine 3";
        const string expected = "Line 1\nLine 2\nLine 3";

        Assert.AreEqual(
            expected,
            Characters.NormalizeLineEndings( windowsStyle ),
            false
        );
    }

    [TestMethod]
    public void GivenOldMacOSEndings_ReturnsOnlyLineFeeds()
    {
        const string oldMacOsStyle = "Line 1\rLine 2\rLine 3";
        const string expected = "Line 1\nLine 2\nLine 3";

        Assert.AreEqual(
            expected,
            Characters.NormalizeLineEndings( oldMacOsStyle ),
            false
        );
    }

    [TestMethod]
    public void GivenWierdEndings_ReturnsOnlyLineFeeds()
    {
        const string wierd = "Line 1\u2028Line 2\u2029Line 3";
        const string expected = "Line 1\nLine 2\nLine 3";

        Assert.AreEqual(
            expected,
            Characters.NormalizeLineEndings( wierd ),
            false
        );
    }

    [TestMethod]
    public void GivenMixedEndings_ReturnsOnlyLineFeeds()
    {
        const string mixed =
            "Line 1\rLine 2\r\nLine 3\nLine4\u2028Line5\r\n\n";
        const string expected =
            "Line 1\nLine 2\nLine 3\nLine4\nLine5\n\n";

        Assert.AreEqual(
            expected,
            Characters.NormalizeLineEndings( mixed ),
            false
        );
    }
}