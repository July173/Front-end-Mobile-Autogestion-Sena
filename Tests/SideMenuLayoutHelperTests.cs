using Xunit;
using FluentAssertions;
using AutogestionSenaMaui.Helpers;

namespace AutogestionSena.MAUI.Tests
{
    public class SideMenuLayoutHelperTests
    {
        [Fact]
        public void ComputeWidth_WithParentWidth_ReturnsHalfOfParent()
        {
            var result = SideMenuLayoutHelper.ComputeWidth(800, 0);
            result.Should().BeApproximately(400, 0.01);
        }

        [Fact]
        public void ComputeWidth_WithParentWidthZero_UsesScreenWidth()
        {
            var result = SideMenuLayoutHelper.ComputeWidth(0, 1080);
            result.Should().BeApproximately(540, 0.01);
        }

        [Fact]
        public void ComputeHeight_WithMainContentHeight_ReturnsMainContentHeight()
        {
            var result = SideMenuLayoutHelper.ComputeHeight(600);
            result.Should().BeApproximately(600, 0.01);
        }

        [Fact]
        public void ComputeHeight_WithZero_ReturnsFallback()
        {
            var result = SideMenuLayoutHelper.ComputeHeight(0);
            result.Should().BeGreaterThan(0);
        }
    }
}
