using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using static Shared.Enums.Enums;

namespace Shared {
    public static class Helper {
        public static bool IsIpAddress(this string ipAddress) {
            var retVal = IPAddress.TryParse(ipAddress, out _);
            return retVal;
        }
        public static bool IsInServiceType(this IEnumerable<string> values) {
            foreach (var value in values) {
                if (!value.DoesEnumExistWithDescription<ServiceType>()) {
                    return false;
                }            }
            return true;
        }
        public static T ParseEnum<T>(this string value) {
            return (T)Enum.Parse(typeof(T), value, true);
        }
        public static IEnumerable<T> ParseEnums<T>(this IEnumerable<string> values) {
            var returnEnumList = new List<T>();
            foreach (var value in values) {
                returnEnumList.Add(value.ParseEnum<T>());
            }
            return returnEnumList;
        }

        public static string GetEnumDescription(this Enum value) {
            var fi = value.GetType().GetField(value.ToString());
            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);
            if (attributes.Length > 0)
                return attributes[0].Description;
            return value.ToString();
        }

        public static T GetEnumFromDescription<T>(this string description) where T : Enum {
            foreach (var field in typeof(T).GetFields()) {
                var attributes = Attribute.GetCustomAttributes(field, typeof(DescriptionAttribute));

                foreach (var attribute in attributes) {
                    if (attribute is DescriptionAttribute descriptionAttribute) {
                        if (descriptionAttribute.Description.Equals(description, StringComparison.OrdinalIgnoreCase)) {
                            return (T)field.GetValue(null);
                        }
                    }
                    else {
                        if (field.Name == description) {
                            return (T)field.GetValue(null);
                        }
                    }
                }
            }
            return default;
        }

        public static bool DoesEnumExistWithDescription<T>(this string description) where T : Enum {
            foreach (var field in typeof(T).GetFields()) {
                var attributes = Attribute.GetCustomAttributes(field, typeof(DescriptionAttribute));

                foreach (var attribute in attributes) {
                    if (attribute is DescriptionAttribute descriptionAttribute) {
                        if (descriptionAttribute.Description.Equals(description, StringComparison.OrdinalIgnoreCase)) {
                            return true;
                        }
                    }
                    else {
                        if (field.Name.Equals(description, StringComparison.OrdinalIgnoreCase)) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
