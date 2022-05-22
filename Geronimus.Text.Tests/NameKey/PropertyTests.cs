namespace Geronimus.Text.Tests;

[TestClass]
public class NameKey_PropertyTests
{
    [TestMethod]
    public void Context_ThrowsWhenTheNameKeyIsARootName()
    {
        NameKey example = new NameKey( "user" );

        Assert.IsTrue( example.IsRoot );
        Assert.ThrowsException<InvalidOperationException>(
            () => { NameKey ctx = example.Context; }
        );
    }

    [TestMethod]
    public void Context_ReturnsTheNameKeyOfTheNamesPrecedingTheLeafName()
    {
        Assert.AreEqual<NameKey>(
            new NameKey( "user/datatypes" ),
            new NameKey( "user/datatypes/url" ).Context
        );
    }

    [TestMethod]
    public void Context_MultipleCallsShouldReturnTheSameInstance()
    {
        NameKey example = new NameKey( "user/datatypes/url" );

        Assert.AreSame( example.Context, example.Context );
    }

    [TestMethod]
    public void HasContext_IsTrueWhenTheNameKeyHasMultipleNames()
    {
        Assert.IsTrue( new NameKey( "user/datatype" ).HasContext );
        Assert.IsFalse( new NameKey( "user" ).HasContext );
    }

    [TestMethod]
    public void IsRoot_IsTrueWhenTheNameKeyHasOnlyOneName()
    {
        Assert.IsTrue( new NameKey( "user" ).IsRoot );
        Assert.IsFalse( new NameKey( "user/datatype" ).IsRoot );
    }

    [TestMethod]
    public void LeafName_AlwaysReturnsTheFinalName()
    {
        Assert.AreEqual( "user", new NameKey( "user" ).LeafName );
        Assert.AreEqual( "datatype", new NameKey( "user/datatype" ).LeafName );
        Assert.AreEqual( "url", new NameKey( "user/datatype/url" ).LeafName );
    }
}
