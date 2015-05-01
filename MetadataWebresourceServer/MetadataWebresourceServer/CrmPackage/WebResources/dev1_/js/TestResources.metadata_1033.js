// This is a test webresource for the SparkleXRM Metadata Server
// Content in this webresource wil be dynamically evaluated and then cached by the client for their particular language

// This content is static
var someStaticValue = 123;

/*metadata
// This content will be dynamically evaluated on first request - and then again when the webresource cache key is changed after a publish
// It is enclosed in the comment tags so that if the metadata server plugin is not installed it does not cause syntax errors

// Account
//<@format=ResourceStrings.{1}_{2}_{3}={0}@>
<@account.DisplayName@>;
<@account.DisplayCollectionName@>;
<@account.name.DisplayName@>;

// Contact
<@contact.DisplayName@>;
<@contact.DisplayCollectionName@>;
<@contact.fullname.DisplayName@>;

//<@format=@>
MultiLanguageView.AddOptionsetMetadata('account','accountcategorycode',<@account.accountcategorycode.OptionSet@>);

var more = <@contact.fullname.DisplayName@>;
var accountDisplayName = <@account.DisplayName@>;

var someFetchXml = <@fetch=<fetch version="1.0" output-format="xml-platform" mapping="logical" distinct="false" count="5">
                  <entity name="account">
                    <attribute name="name" />
                    <attribute name="primarycontactid" />
                    <attribute name="telephone1" />
                    <attribute name="accountid" />
                   <order attribute="name" descending="false" />
                  </entity>
                </fetch>@>;


metadata*/