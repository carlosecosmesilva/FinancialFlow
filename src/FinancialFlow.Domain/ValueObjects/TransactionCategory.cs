using System;
using FinancialFlow.Domain.Enums;

namespace FinancialFlow.Domain.ValueObjects
{
    /// <summary>
    /// Value Object imutável que representa uma categoria de transação.
    /// </summary>
    public sealed record TransactionCategory
    {
        public string Name { get; init; }
        public CategoryType Type { get; init; }
        public string Color { get; init; }

        public TransactionCategory(string name, CategoryType type, string color = "#808080")
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name is required.", nameof(name));

            if (string.IsNullOrWhiteSpace(color))
                throw new ArgumentException("Color is required.", nameof(color));

            Name = name.Trim();
            Type = type;
            Color = color.Trim();
        }

        /// <summary>
        /// Cria uma categoria de moradia.
        /// </summary>
        public static TransactionCategory Housing(string name = "Moradia")
            => new(name, CategoryType.Housing, "#FF6B6B");

        /// <summary>
        /// Cria uma categoria de transporte.
        /// </summary>
        public static TransactionCategory Transportation(string name = "Transporte")
            => new(name, CategoryType.Transportation, "#4ECDC4");

        /// <summary>
        /// Cria uma categoria de alimentação.
        /// </summary>
        public static TransactionCategory Food(string name = "Alimentação")
            => new(name, CategoryType.Food, "#45B7D1");

        /// <summary>
        /// Cria uma categoria de saúde.
        /// </summary>
        public static TransactionCategory Health(string name = "Saúde")
            => new(name, CategoryType.Health, "#96CEB4");

        /// <summary>
        /// Cria uma categoria de educação.
        /// </summary>
        public static TransactionCategory Education(string name = "Educação")
            => new(name, CategoryType.Education, "#FFEAA7");

        /// <summary>
        /// Cria uma categoria de lazer.
        /// </summary>
        public static TransactionCategory Leisure(string name = "Lazer")
            => new(name, CategoryType.Leisure, "#DFE6E9");

        /// <summary>
        /// Cria uma categoria de salário.
        /// </summary>
        public static TransactionCategory Salary(string name = "Salário")
            => new(name, CategoryType.Salary, "#00B894");

        /// <summary>
        /// Cria uma categoria de investimentos.
        /// </summary>
        public static TransactionCategory Investments(string name = "Investimentos")
            => new(name, CategoryType.Investments, "#6C5CE7");

        public override string ToString() => $"{Name} ({Type})";
    }
}
