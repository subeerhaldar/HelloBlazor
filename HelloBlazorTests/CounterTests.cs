using Xunit;
using HelloBlazor.Pages;
using Bunit;

namespace HelloBlazorTests
{
    public class CounterTests : TestContext
    {
        [Fact]
        public void CounterShouldIncrementWhenSelected()
        {
            // Arrange
            //using var ctx = new Bunit.TestContext();
            var cut = RenderComponent<Counter>();
            var paraElm = cut.Find("p");

            // Act
            cut.Find("button").Click();
            var paraElmText = paraElm.TextContent;

            // Assert
            paraElmText.MarkupMatches("Current count: 1");
        }
    }
}
