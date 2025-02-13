using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InterviewMauiBlazor.ViewModels
{
    public class TransactionViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Product is required.")]
        public int? ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        [Required(ErrorMessage = "Buyer is required.", AllowEmptyStrings = false)]
        public string Buyer { get; set; }

        [Required(ErrorMessage = "Seller is required.", AllowEmptyStrings = false)]
        public string Seller { get; set; }

        [Required(ErrorMessage = "Time is required.")]
        public DateTime? Time { get; set; }

        [Required(ErrorMessage = "Status is required.", AllowEmptyStrings = false)]
        public string Status { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrWhiteSpace(Buyer) && !string.IsNullOrWhiteSpace(Seller) &&
                Buyer.Equals(Seller, StringComparison.OrdinalIgnoreCase))
            {
                yield return new ValidationResult("Buyer and Seller must be different.", new[] { nameof(Buyer), nameof(Seller) });
            }
        }
    }
}
