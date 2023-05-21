using System.ComponentModel;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace eVoucher.Handlers
{
    public class eVoucherHelper
    {
        public const string EmailAddressPattern = @"^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$";
        private const int US_PHONE_LENGTH = 11;
        public static string GetEnumDescription(Enum value)
        {
            var enumMember = value.GetType().GetMember(value.ToString()).FirstOrDefault();
            var descriptionAttribute =
                enumMember == null
                    ? default(DescriptionAttribute)
                    : enumMember.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
            return
                descriptionAttribute == null
                    ? value.ToString()
                    : descriptionAttribute.Description;
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}

