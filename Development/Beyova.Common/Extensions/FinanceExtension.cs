using System.Collections.Generic;
using System.Linq;

namespace Beyova
{
    /// <summary>
    /// Extensions FinanceExtension
    /// </summary>
    public static class FinanceExtension
    {
        /// <summary>
        /// Calculates the VAT. Example: total price = 80, VAT=7%, then VAT = 7% * 80/(1+7%) = 5.23364486
        /// </summary>
        /// <param name="retailPrice">The retail price.</param>
        /// <param name="vatRate">The vat rate.</param>
        /// <returns>System.Decimal.</returns>
        public static decimal CalculateVAT(this decimal retailPrice, decimal vatRate)
        {
            return vatRate * (retailPrice / (1 + vatRate));
        }

        /// <summary>
        /// Generates the finance item detail.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="actualUnitRetailPrice">The unit retail price.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="vatRate">The vat rate.</param>
        /// <param name="actualDiscount">The actual discount.</param>
        /// <param name="currency">The currency.</param>
        /// <returns>FinanceItemDetail.</returns>
        public static FinancialItemDetail GenerateFinanceItemDetail(string name, decimal actualUnitRetailPrice, decimal amount, decimal vatRate = 0, decimal actualDiscount = 0, string currency = "CNY")
        {
            var totalRetailPrice = actualUnitRetailPrice * amount - actualDiscount;
            var vat = CalculateVAT(totalRetailPrice, vatRate);
            var totalCost = totalRetailPrice - vat + actualDiscount;
            var unitPrice = amount == 0 ? 0 : totalCost / amount;

            return new FinancialItemDetail
            {
                Name = name,
                Amount = amount,
                Currency = currency,
                Discount = actualDiscount,
                Price = unitPrice,
                TotalPrice = totalRetailPrice,
                VATRate = vatRate,
                VAT = vat
            };
        }

        /// <summary>
        /// Converts list of <see cref="FinancialItemDetail"/> as <see cref="FinancialItemSummary"/>.
        /// </summary>
        /// <param name="details">The details.</param>
        /// <param name="summaryName">Name of the summary.</param>
        /// <returns>FinancialItemSummary.</returns>
        /// <exception cref="InvalidObjectException">Currency</exception>
        public static FinancialItemSummary AsSummary(this IEnumerable<FinancialItemDetail> details, string summaryName)
        {
            FinancialItemSummary result = null;

            if (details.HasItem())
            {
                result = new FinancialItemSummary
                {
                    Name = summaryName,
                    Currency = details.First().Currency.SafeToUpper()
                };

                foreach (var one in details)
                {
                    if (!one.Currency.MeaningfulEquals(result.Currency, System.StringComparison.InvariantCultureIgnoreCase))
                    {
                        throw ExceptionFactory.CreateInvalidObjectException(nameof(one.Currency), data: new { currency1 = result.Currency, currency2 = one.Currency }, reason: "Inconsistance currency.");
                    }

                    result.TotalPrice += one.TotalPrice;
                    result.TotalDiscount += one.Discount;
                    result.Amount += one.Amount;
                    result.VAT += one.VAT;
                    result.Details.Add(one);
                }
            }

            return result;
        }
    }
}