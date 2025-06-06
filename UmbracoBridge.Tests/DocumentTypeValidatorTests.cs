using UmbracoBridge.Models;
using UmbracoBridge.Validators;

namespace UmbracoBridge.Tests
{
    public class DocumentTypeValidatorTests
    {
        [Fact]
        public void Should_Validate_Correct_Input()
        {
            var request = new DocumentTypeCreateRequest
            {
                Alias = "testAlias",
                Name = "Test Name",
                Description = "Description",
                Icon = "icon-doc"
            };

            var (isValid, errors) = DocumentTypeValidator.Validate(request);
            Assert.True(isValid);
            Assert.Empty(errors);
        }

        [Fact]
        public void Should_Fail_When_Fields_Invalid()
        {
            var request = new DocumentTypeCreateRequest
            {
                Alias = "",
                Name = "",
                Description = "",
                Icon = "wrongIcon"
            };

            var (isValid, errors) = DocumentTypeValidator.Validate(request);
            Assert.False(isValid);
            Assert.Equal(4, errors.Count);
        }
    }
}
