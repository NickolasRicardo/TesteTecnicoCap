using Questao5.Domain.Enumerators;
using Questao5.Domain.Extensions;

namespace Questao5.Test.Extensions
{
    public class EnumExtensionsTests
    {
        [Fact]
        public void GetDescription_EnumWithDescription_ReturnsDescription()
        {
            // Arrange
            var enumValue = TipoMovimentoEnum.Credito;

            // Act
            var description = enumValue.GetDescription();

            // Assert
            Assert.Equal("C", description);
        }

        [Fact]
        public void GetDescription_EnumWithoutDescription_ReturnsEnumValueToString()
        {
            // Arrange
            var enumValue = TipoMovimentoEnum.Debito;

            // Act
            var description = enumValue.GetDescription();

            // Assert
            Assert.Equal("D", description);
        }

        [Fact]
        public void GetEnumValueFromDescription_DescriptionExists_ReturnsEnumValue()
        {
            // Arrange
            var description = "C";

            // Act
            var enumValue = description.GetEnumValueFromDescription<TipoMovimentoEnum>();

            // Assert
            Assert.Equal(TipoMovimentoEnum.Credito, enumValue);
        }

        [Fact]
        public void GetEnumValueFromDescription_DescriptionDoesNotExist_ThrowsArgumentException()
        {
            // Arrange
            var description = "InvalidDescription";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => description.GetEnumValueFromDescription<TipoMovimentoEnum>());
        }
    }
}
