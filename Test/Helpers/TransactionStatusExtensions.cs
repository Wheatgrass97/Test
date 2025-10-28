using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Test.Models;

namespace Test.Helpers
{
    public static class TransactionStatusExtensions
    {
        public static bool TryParse(string input, out TransactionStatus status)
        {
            status = default;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            var trimmed = input.Trim();

            // enum name (case-insensitive)
            if (Enum.TryParse<TransactionStatus>(trimmed, ignoreCase: true, out status))
                return true;

            var type = typeof(TransactionStatus);

            // DisplayAttribute(Name)
            foreach (var value in Enum.GetValues(type).Cast<Enum>())
            {
                var member = type.GetMember(value.ToString()).FirstOrDefault();
                if (member == null)
                    continue;

                var display = member.GetCustomAttribute<DisplayAttribute>();
                if (display != null && string.Equals(display.Name, trimmed, StringComparison.OrdinalIgnoreCase))
                {
                    status = (TransactionStatus)Convert.ToInt32(value);
                    return true;
                }
            }

            // numeric parse
            if (int.TryParse(trimmed, out var numeric) && Enum.IsDefined(type, numeric))
            {
                status = (TransactionStatus)numeric;
                return true;
            }

            return false;
        }

        public static TransactionStatus ParseOrDefault(string input, TransactionStatus defaultValue = default)
        {
            return TryParse(input, out var result) ? result : defaultValue;
        }
    }
}

