using System;
using System.Runtime.CompilerServices;
using Xrm.Sdk;
using Xrm.Services;

namespace Xrm
{
    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class NumberFormatInfo
    {
        public int Precision;
        public Number MinValue;
        public Number MaxValue;
        public string DecimalSymbol;
        public string NumberGroupFormat;
        public string NumberSepartor;
        public int? NegativeFormatCode;
        public string CurrencySymbol;

    }

    public static class NumberEx
    {
        public static Number Parse(string value, NumberFormatInfo format)
        {
            if (String.IsNullOrEmpty(value))
                return null;

            // Remove spaces
            value = value.Replace(" ", "");

            // Remove groupseparators and substitute the decimal separator
            if (format.DecimalSymbol != ".")
            {
                value = value.Replace(format.DecimalSymbol, ".");
            }
            value = value.Replace(format.NumberSepartor, "");

            // Remove negative formatting
            if (value.StartsWith("("))
            {
                value = "-" + value.Replace("(", "").Replace(")", "");
            }
            else if (value.EndsWith("-"))
            {
                value = "-" + value.Substring(0, value.Length - 1);
            }
            Number numericValue = Number.Parse(value);
            return numericValue;
        }

        public static NumberFormatInfo GetNumberFormatInfo()
        {
            NumberFormatInfo format = new NumberFormatInfo();

            if (OrganizationServiceProxy.UserSettings != null)
            {
                format.DecimalSymbol = OrganizationServiceProxy.UserSettings.DecimalSymbol;
                format.NumberGroupFormat = OrganizationServiceProxy.UserSettings.NumberGroupFormat;
                format.NumberSepartor = OrganizationServiceProxy.UserSettings.NumberSeparator;
                format.NegativeFormatCode = OrganizationServiceProxy.UserSettings.NegativeFormatCode;
            }
            else
            {
                format.DecimalSymbol = ".";
                format.NumberGroupFormat = "3";
                format.NumberSepartor = ",";
                format.NegativeFormatCode = 0;
            }
            // defaults
            format.Precision = 2;
            format.MinValue = -2147483648;
            format.MaxValue = 2147483648;
            return format;
        }
        public static NumberFormatInfo GetCurrencyEditFormatInfo()
        {
            NumberFormatInfo format = new NumberFormatInfo();

            if (OrganizationServiceProxy.UserSettings != null)
            {
                format.DecimalSymbol = OrganizationServiceProxy.UserSettings.DecimalSymbol;
                format.NumberGroupFormat = OrganizationServiceProxy.UserSettings.NumberGroupFormat;
                format.NumberSepartor = OrganizationServiceProxy.UserSettings.NumberSeparator;
                format.NegativeFormatCode = OrganizationServiceProxy.UserSettings.NegativeCurrencyFormatCode;
                format.Precision = OrganizationServiceProxy.UserSettings.CurrencyDecimalPrecision.Value;
                format.CurrencySymbol = OrganizationServiceProxy.UserSettings.CurrencySymbol;
            }
            else
            {
                format.DecimalSymbol = ".";
                format.NumberGroupFormat = "3";
                format.NumberSepartor = ",";
                format.NegativeFormatCode = 0;
                format.Precision = 2;
                format.CurrencySymbol = "$";
            }
            return format;
        }
        public static NumberFormatInfo GetCurrencyFormatInfo()
        {
            NumberFormatInfo format = new NumberFormatInfo();

            if (OrganizationServiceProxy.UserSettings != null)
            {
                format.DecimalSymbol = OrganizationServiceProxy.UserSettings.DecimalSymbol;
                format.NumberGroupFormat = OrganizationServiceProxy.UserSettings.NumberGroupFormat;
                format.NumberSepartor = OrganizationServiceProxy.UserSettings.NumberSeparator;
                format.NegativeFormatCode = OrganizationServiceProxy.UserSettings.NegativeCurrencyFormatCode;
                format.Precision = OrganizationServiceProxy.UserSettings.CurrencyDecimalPrecision.Value;
                format.CurrencySymbol = OrganizationServiceProxy.UserSettings.CurrencySymbol;
            }
            else
            {
                format.DecimalSymbol = ".";
                format.NumberGroupFormat = "3";
                format.NumberSepartor = ",";
                format.NegativeFormatCode = 0;
                format.Precision = 2;
                format.CurrencySymbol = "$";
            }
            return format;
        }

        public static string Format(Number value,NumberFormatInfo format)
        {
            if (value == null)
                return String.Empty;

           string[] numberGroupFormats = format.NumberGroupFormat.Split(",");
           string formattedNumber = "";

           int wholeNumber = Math.Floor(Math.Abs(value));
           string wholeNumberString = wholeNumber.ToString();
           string decimalPartString = value.ToString().Substr(wholeNumberString.Length + 1 + (value < 0 ? 1 : 0)); // Fixes Issue #22 - Thanks to @advancedhair
           
           int i = wholeNumberString.Length;
                int j = 0;

                while (i > 0)
                {
                    int groupSize = int.Parse(numberGroupFormats[j]);

                    // If there are more number group formats, get the next one, otherwise stick with this one until the end of the formatting
                    if (j < (numberGroupFormats.Length-1))
                        j++;

                    if (groupSize == 0)
                        groupSize = i + 1;

                    formattedNumber = wholeNumberString.Substring(i, i - groupSize) + formattedNumber;

                    if (i > groupSize)
                        formattedNumber = format.NumberSepartor + formattedNumber;

                    i = i - groupSize;
                }

                // Add decimal part
                bool neg = (value < 0);

              
                
                if (format.Precision > 0)
                {
                    // Add padding zeros
                   
                    int paddingRequired = format.Precision - decimalPartString.Length;
                    for (int d = 0; d < paddingRequired; d++)
                    {
                        decimalPartString = decimalPartString + "0";
                    }

                    formattedNumber = formattedNumber + format.DecimalSymbol + decimalPartString;
                }

                // Format negative
                if (neg)
                {
                    switch (format.NegativeFormatCode)
                    {
                        case 0:
                            formattedNumber = "(" + formattedNumber + ")";
                            break;
                     
                        case 2:
                            formattedNumber = "- " + formattedNumber;
                            break;
                        case 3:
                            formattedNumber = formattedNumber + "-";
                            break;
                        case 4:
                            formattedNumber = formattedNumber + " -";
                            break;
                        case 1:
                        default:
                            formattedNumber = "-" + formattedNumber;
                            break;
                    }
                }


                return formattedNumber;
        }

        public static Number Round(Number numericValue, int precision)
        {
            int precisionMultiplier = 1;
            if (precision > 0)
            {
                precisionMultiplier = Math.Pow(10,precision);
            }
            return Math.Round(numericValue * precisionMultiplier) / precisionMultiplier;
            
        }
        public static string GetCurrencySymbol(Guid currencyId)
        {
            // Lookup the currency code/symbol
            EntityCollection orgSettings = CachedOrganizationService.RetrieveMultiple(@"<fetch distinct='false' no-lock='false' mapping='logical'><entity name='organization'><attribute name='currencydisplayoption' /><attribute name='currencysymbol' /></entity></fetch>");
            Entity orgSetting = orgSettings.Entities[0];
            Entity currency = CachedOrganizationService.Retrieve("transactioncurrency", currencyId.ToString(), new string[] { "currencysymbol", "isocurrencycode" });
            if (orgSetting.GetAttributeValueOptionSet("currencydisplayoption").Value == 0) // Fixes Issue #23 - Thanks @advancedhair
            {
                // Currency Symbol
                return currency.GetAttributeValueString("currencysymbol") + " ";
            }
            else
            {
                // Currency ISO Code
                return currency.GetAttributeValueString("isocurrencycode") + " ";
            }
        }
    }
}
