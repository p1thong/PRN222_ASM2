using System;

namespace ASM1.Service.Utilities
{
    public static class PromotionCodeGenerator
    {
        /// <summary>
        /// Tạo promotion code cho customer mới
        /// Format: WELCOME-{CustomerId}-{RandomString}
        /// </summary>
        public static string GenerateWelcomeCode(int customerId)
        {
            var randomPart = Guid.NewGuid().ToString("N")[..6].ToUpper();
            return $"WELCOME-{customerId}-{randomPart}";
        }

        /// <summary>
        /// Tính discount amount dựa trên order amount và discount percent
        /// </summary>
        public static decimal CalculateDiscountAmount(decimal orderAmount, decimal discountPercent)
        {
            return Math.Round(orderAmount * discountPercent / 100, 2);
        }

        /// <summary>
        /// Kiểm tra xem promotion code có phải là welcome code không
        /// </summary>
        public static bool IsWelcomeCode(string promotionCode)
        {
            return promotionCode?.StartsWith("WELCOME-") == true;
        }

        /// <summary>
        /// Lấy customer ID từ welcome promotion code
        /// </summary>
        public static int? GetCustomerIdFromWelcomeCode(string promotionCode)
        {
            if (!IsWelcomeCode(promotionCode))
                return null;

            var parts = promotionCode.Split('-');
            if (parts.Length >= 2 && int.TryParse(parts[1], out int customerId))
                return customerId;

            return null;
        }
    }
}