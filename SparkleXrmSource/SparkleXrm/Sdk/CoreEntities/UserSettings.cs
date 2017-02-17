// UserSettings.cs
//

using System.Runtime.CompilerServices;

namespace Xrm.Sdk
{
    [ScriptNamespace("SparkleXrm.Sdk")]
    public class UserSettingsAttributes
    {

        public static string UserSettingsId = "usersettingsid";

        public static string BusinessUnitId = "businessunitid";

        public static string CalendarType = "calendartype";

        public static string CurrencyDecimalPrecision = "currencydecimalprecision";

        public static string CurrencyFormatCode = "currencyformatcode";

        public static string CurrencySymbol = "currencysymbol";

        public static string DateFormatCode = "dateformatcode";

        public static string DateFormatString = "dateformatstring";

        public static string DateSeparator = "dateseparator";

        public static string DecimalSymbol = "decimalsymbol";

        public static string DefaultCalendarView = "defaultcalendarview";

        public static string DefaultDashboardId = "defaultdashboardid";

        public static string LocaleId = "localeid";

        public static string LongDateFormatCode = "longdateformatcode";

        public static string NegativeCurrencyFormatCode = "negativecurrencyformatcode";

        public static string NegativeFormatCode = "negativeformatcode";

        public static string NumberGroupFormat = "numbergroupformat";

        public static string NumberSeparator = "numberseparator";

        public static string OfflineSyncInterval = "offlinesyncinterval";

        public static string PricingDecimalPrecision = "pricingdecimalprecision";

        public static string ShowWeekNumber = "showweeknumber";

        public static string SystemUserId = "systemuserid";

        public static string TimeFormatCodestring = "timeformatcodestring";

        public static string TimeFormatString = "timeformatstring";

        public static string TimeSeparator = "timeseparator";

        public static string TimeZoneBias = "timezonebias";

        public static string TimeZoneCode = "timezonecode";

        public static string TimeZoneDaylightBias = "timezonedaylightbias";

        public static string TimeZoneDaylightDay = "timezonedaylightday";

        public static string TimeZoneDaylightDayOfWeek = "timezonedaylightdayofweek";

        public static string TimeZoneDaylightHour = "timezonedaylighthour";

        public static string TimeZoneDaylightMinute = "timezonedaylightminute";

        public static string TimeZoneDaylightMonth = "timezonedaylightmonth";

        public static string TimeZoneDaylightSecond = "timezonedaylightsecond";

        public static string TimeZoneDaylightYear = "timezonedaylightyear";

        public static string TimeZoneStandardBias = "timezonestandardbias";

        public static string TimeZoneStandardDay = "timezonestandardday";

        public static string TimeZoneStandardDayOfWeek = "timezonestandarddayofweek";

        public static string TimeZoneStandardHour = "timezonestandardhour";

        public static string TimeZoneStandardMinute = "timezonestandardminute";

        public static string TimeZoneStandardMonth = "timezonestandardmonth";

        public static string TimeZoneStandardSecond = "timezonestandardsecond";

        public static string TimeZoneStandardYear = "timezonestandardyear";

        public static string TransactionCurrencyId = "transactioncurrencyid";

        public static string UILanguageId = "uilanguageid";

        public static string WorkdayStartTime = "workdaystarttime";

        public static string WorkdayStopTime = "workdaystoptime";

    }

    [ScriptNamespace("SparkleXrm.Sdk")]
    public partial class UserSettings : Entity
    {
        public static string EntityLogicalName = "usersettings";

        public UserSettings()
            : base(EntityLogicalName)
        {
        }
        [ScriptName("usersettingsid")]
        public Guid UserSettingsId;
        [ScriptName("businessunitid")]
        public Guid BusinessUnitId;
        [ScriptName("calendartype")]
        public int? CalendarType;


        [ScriptName("currencydecimalprecision")]
        public int? CurrencyDecimalPrecision;
        [ScriptName("currencyformatcode")]
        public int? CurrencyFormatCode;
        [ScriptName("currencysymbol")]
        public string CurrencySymbol;

        [ScriptName("dateformatcode")]
        public int? DateFormatCode;
        [ScriptName("dateformatstring")]
        public string DateFormatString;
        [ScriptName("dateseparator")]
        public string DateSeparator;
        [ScriptName("decimalsymbol")]
        public string DecimalSymbol;
        [ScriptName("defaultcalendarview")]
        public int? DefaultCalendarView;
        [ScriptName("defaultdashboardid")]
        public Guid DefaultDashboardId;
        [ScriptName("localeid")]
        public int? LocaleId;
        [ScriptName("longdateformatcode")]
        public int? LongDateFormatCode;
        [ScriptName("negativecurrencyformatcode")]
        public int? NegativeCurrencyFormatCode;
        [ScriptName("negativeformatcode")]
        public int? NegativeFormatCode;
        [ScriptName("numbergroupformat")]
        public string NumberGroupFormat;
        [ScriptName("numberseparator")]
        public string NumberSeparator;
        [ScriptName("offlinesyncinterval")]
        public int? OfflineSyncInterval;
        [ScriptName("pricingdecimalprecision")]
        public int? PricingDecimalPrecision;
        [ScriptName("showweeknumber")]
        public bool? ShowWeekNumber;
        [ScriptName("systemuserid")]
        public Guid SystemUserId;
        [ScriptName("timeformatcodestring")]
        public int? TimeFormatCodestring;
        [ScriptName("timeformatstring")]
        public string TimeFormatString;
        [ScriptName("timeseparator")]
        public string TimeSeparator;
        [ScriptName("timezonebias")]
        public int? TimeZoneBias;
        [ScriptName("timezonecode")]
        public int? TimeZoneCode;
        [ScriptName("timezonedaylightbias")]
        public int? TimeZoneDaylightBias;
        [ScriptName("timezonedaylightday")]
        public int? TimeZoneDaylightDay;
        [ScriptName("timezonedaylightdayofweek")]
        public int? TimeZoneDaylightDayOfWeek;
        [ScriptName("timezonedaylighthour")]
        public int? TimeZoneDaylightHour;
        [ScriptName("timezonedaylightminute")]
        public int? TimeZoneDaylightMinute;
        [ScriptName("timezonedaylightmonth")]
        public int? TimeZoneDaylightMonth;
        [ScriptName("timezonedaylightsecond")]
        public int? TimeZoneDaylightSecond;
        [ScriptName("timezonedaylightyear")]
        public int? TimeZoneDaylightYear;
        [ScriptName("timezonestandardbias")]
        public int? TimeZoneStandardBias;

        [ScriptName("timezonestandardday")]
        public int? TimeZoneStandardDay;
        [ScriptName("timezonestandarddayofweek")]
        public int? TimeZoneStandardDayOfWeek;
        [ScriptName("timezonestandardhour")]
        public int? TimeZoneStandardHour;
        [ScriptName("timezonestandardminute")]
        public int? TimeZoneStandardMinute;
        [ScriptName("timezonestandardmonth")]
        public int? TimeZoneStandardMonth;
        [ScriptName("timezonestandardsecond")]
        public int? TimeZoneStandardSecond;
        [ScriptName("timezonestandardyear")]
        public int? TimeZoneStandardYear;
        [ScriptName("transactioncurrencyid")]
        public EntityReference TransactionCurrencyId;
        [ScriptName("uilanguageid")]
        public int? UILanguageId;
        [ScriptName("workdaystarttime")]
        public string WorkdayStartTime;
        [ScriptName("workdaystoptime")]
        public string WorkdayStopTime;

        public string GetNumberFormatString(int decimalPlaces)
        {

            return "###,###,###.000";

        }
    }
}