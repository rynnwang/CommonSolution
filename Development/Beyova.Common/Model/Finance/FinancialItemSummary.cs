using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class FinancialItemSummary.
    /// </summary>
    public class FinancialItemSummary
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>The currency.</value>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the vat.
        /// </summary>
        /// <value>The vat.</value>
        public decimal VAT { get; set; }

        /// <summary>
        /// Gets or sets the total price.
        /// </summary>
        /// <value>The total price.</value>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Gets or sets the discount.
        /// </summary>
        /// <value>The discount.</value>
        public decimal TotalDiscount { get; set; }

        /// <summary>
        /// Gets or sets the details.
        /// </summary>
        /// <value>The details.</value>
        public List<FinancialItemDetail> Details { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialItemSummary"/> class.
        /// </summary>
        public FinancialItemSummary()
        {
            this.Details = new List<FinancialItemDetail>();
        }
    }
}