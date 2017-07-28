

declare module SparkleXrm {

    export interface IEnumerable {
    }
    export interface IEnumerableFunc extends Function {
        prototype: IEnumerable;
        foreach<T>(any, itterator: { (item: T): void }): void;
    }
    var IEnumerable: IEnumerableFunc;

    export interface NumberEx {
    }
    export interface NumberExFunc extends Function {
        prototype: NumberEx;
        new (): NumberEx;
        parse(value: string, format: {precision: number, minValue: number, maxValue: number, decimalSymbol: string, numberGroupFormat: string, numberSepartor: string, negativeFormatCode: number, currencySymbol: string}): number;
        getNumberFormatInfo(): {precision: number, minValue: number, maxValue: number, decimalSymbol: string, numberGroupFormat: string, numberSepartor: string, negativeFormatCode: number, currencySymbol: string};
        getCurrencyEditFormatInfo(): {precision: number, minValue: number, maxValue: number, decimalSymbol: string, numberGroupFormat: string, numberSepartor: string, negativeFormatCode: number, currencySymbol: string};
        getCurrencyFormatInfo(): {precision: number, minValue: number, maxValue: number, decimalSymbol: string, numberGroupFormat: string, numberSepartor: string, negativeFormatCode: number, currencySymbol: string};
        format(value: number, format: {precision: number, minValue: number, maxValue: number, decimalSymbol: string, numberGroupFormat: string, numberSepartor: string, negativeFormatCode: number, currencySymbol: string}): string;
        round(numericValue: number, precision: number): number;
        getCurrencySymbol(currencyId: SparkleXrm.Sdk.Guid): string;
    }
    var NumberEx: NumberExFunc;

   
    export interface StringEx {
    }
    export interface StringExFunc extends Function {
        prototype: StringEx;
        new (): StringEx;
        IN(value: string, values: string[]): boolean;
    }
    var StringEx: StringExFunc;
}

declare module SparkleXrm.ComponentModel {
    export interface INotifyPropertyChanged {
        SparkleXrm$ComponentModel$INotifyPropertyChanged$addpropertyChanged(value: {(sender: any, e: {empty: any, propertyName: string}): void}): void;
        SparkleXrm$ComponentModel$INotifyPropertyChanged$removepropertyChanged(value: {(sender: any, e: {empty: any, propertyName: string}): void}): void;
        addpropertyChanged(value: {(sender: any, e: {empty: any, propertyName: string}): void}): void;
        removepropertyChanged(value: {(sender: any, e: {empty: any, propertyName: string}): void}): void;
        SparkleXrm$ComponentModel$INotifyPropertyChanged$raisePropertyChanged(propertyName: string): void;
        raisePropertyChanged(propertyName: string): void;
    }
}

declare module SparkleXrm.Sdk {
    export interface Guid {
        value: string;
        toString(): string;
    }
    export interface GuidFunc extends Function {
        prototype: Guid;
        new (Value: string): Guid;
        empty: SparkleXrm.Sdk.Guid;
    }
    var Guid: GuidFunc;

    export interface Entity extends SparkleXrm.ComponentModel.INotifyPropertyChanged {
        logicalName: string;
        id: string;
        entityState: SparkleXrm.Sdk.EntityStates;
        formattedValues: any; //TODO:System.Collections.Generic.Dictionary<string,string>;
        relatedEntities: any; //TODO:System.Collections.Generic.Dictionary<string,SparkleXrm.Sdk.EntityCollection>;
        addpropertyChanged(value: {(sender: any, e: {empty: any, propertyName: string}): void}): void;
        removepropertyChanged(value: {(sender: any, e: {empty: any, propertyName: string}): void}): void;
        setAttributeValue(name: string, value: any): void;
        getAttributeValue<T>(attributeName: string):T;
        getAttributeValueOptionSet(attributeName: string): SparkleXrm.Sdk.OptionSetValue;
        getAttributeValueGuid(attributeName: string): SparkleXrm.Sdk.Guid;
        getAttributeValueInt(attributeName: string): number;
        getAttributeValueFloat(attributeName: string): number;
        getAttributeValueString(attributeName: string): string;
        getAttributeValueEntityReference(attributeName: string): SparkleXrm.Sdk.EntityReference;
        raisePropertyChanged(propertyName: string): void;
        toEntityReference(): SparkleXrm.Sdk.EntityReference;
    }
    export interface EntityFunc extends Function {
        prototype: Entity;
        new (entityName: string): Entity;
        sortDelegate(attributeName: string, a: SparkleXrm.Sdk.Entity, b: SparkleXrm.Sdk.Entity): number;
    }
    var Entity: EntityFunc;

    export interface XmlHelper {
    }
    

    export interface RequestResponse {
        entityLogicalName: string;
        response: string;
    }
    export interface RequestResponseFunc extends Function {
        prototype: RequestResponse;
        new (): RequestResponse;
    }
    var RequestResponse: RequestResponseFunc;

    export interface Relationship {
        primaryEntityRole: SparkleXrm.Sdk.EntityRole;
        schemaName: string;
    }
    export interface RelationshipFunc extends Function {
        prototype: Relationship;
        new (schemaName: string): Relationship;
    }
    var Relationship: RelationshipFunc;

    export interface OrganizationServiceProxy {
    }

    export interface WebApiOrganizationServiceProxy {

    }
    export interface WebApiOrganizationServiceProxyFunc extends Function {
        addNavigationPropertyMetadata(entityLogicalName: string, attributeLogicalName: string, navigationProperties: string)
    }

    var WebApiOrganizationServiceProxy: WebApiOrganizationServiceProxyFunc;

    export interface UserSettings extends Entity {
        
        userSettingsId: Guid;
        businessUnitId: Guid;
        calendarType: number;
        currencyDecimalPrecision: number;
        currencyFormatCode: number;
        currencySymbol: string;
        dateFormatCode: number;
        dateFormatString: string;
        dateSeparator: string;
        decimalSymbol: string;
        defaultCalendarView: number;
        defaultDashboardId: Guid;
        localeId: number;
        longDateFormatCode: number;
        negativeCurrencyFormatCode: number;
        negativeFormatCode: number;
        numberGroupFormat: string;
        numberSeparator: string;
        offlineSyncInterval: number;
        pricingDecimalPrecision: number;
        showWeekNumber: boolean;
        systemUserId: Guid;
        timeFormatCodestring: number;
        timeFormatString: string;
        timeSeparator: string;
        timeZoneBias: number;
        timeZoneCode: number;
        timeZoneDaylightBias: number;
        timeZoneDaylightDay: number;
        timeZoneDaylightDayOfWeek: number;
        timeZoneDaylightHour: number;
        timeZoneDaylightMinute: number;
        timeZoneDaylightMonth: number;
        timeZoneDaylightSecond: number;
        timeZoneDaylightYear: number;
        timeZoneStandardBias: number;
        timeZoneStandardDay: number;
        timeZoneStandardDayOfWeek: number;
        timeZoneStandardHour: number;
        timeZoneStandardMinute: number;
        timeZoneStandardMonth: number;
        timeZoneStandardSecond: number;
        timeZoneStandardYear: number;
        transactionCurrencyId: EntityReference;
        uILanguageId: number;
        workdayStartTime: string;
        workdayStopTime: string;
        getNumberFormatString(decimalPlaces: number);
    }

    export interface OrganizationSettings extends Entity {
        weekStartDayCode: OptionSetValue;
    }

    type ServiceType = "soap2011" | "webApi" ;
   
    export interface OrganizationResponse {

    }

    export interface OrganizationRequest {
        serialise?(): string;
    }

    export interface OrganizationServiceProxyFunc extends Function {
        prototype: OrganizationServiceProxy;
        new (): OrganizationServiceProxy;
        userSettings: UserSettings;
        organizationSettings: OrganizationSettings;
        setImplementation(type: ServiceType);
        registerExecuteMessageResponseType(responseTypeName: string, organizationResponseType: Function): void ;
        getUserSettings(): UserSettings ;
        doesNNAssociationExist(relationship: Relationship, Entity1: EntityReference, Entity2: EntityReference): boolean ;
        associate(entityName: string, entityId: Guid, relationship: Relationship, relatedEntities: EntityReference[]): void ;
        beginAssociate(entityName: string, entityId: Guid, relationship: Relationship, relatedEntities: EntityReference[], callBack: (state: Object) => void): void ;
        endAssociate(asyncState: Object): void ;
        disassociate(entityName: string, entityId: Guid, relationship: Relationship, relatedEntities: EntityReference[]): void ;
        beginDisassociate(entityName: string, entityId: Guid, relationship: Relationship, relatedEntities:EntityReference[], callBack: (state: Object) => void): void ;
        endDisassociate(asyncState: Object): void ;
        retrieveMultiple(fetchXml: string): EntityCollection ;
        beginRetrieveMultiple(fetchXml: string, callBack: (state: Object) => void): void ;
        endRetrieveMultiple(asyncState: Object, entityType: Function): EntityCollection ;
        retrieve(entityName: string, entityId: string, attributesList: string[]): Entity ;
        beginRetrieve(entityName: string, entityId: string, attributesList: string[], callBack: (state: Object) => void): void ;
        endRetrieve(asyncState: Object, entityType: Function): Entity ;
        create(entity: Entity): Guid ;
        beginCreate(entity: Entity, callBack: (state: Object) => void): void ;
        endCreate(asyncState: Object): Guid ;
        setState(id: Guid, entityName: string, stateCode: number, statusCode: number): void ;
        beginSetState(id: Guid, entityName: string, stateCode: number, statusCode: number, callBack: (state: Object) => void): void ;
        endSetState(asyncState: Object): void ;
        delete_(entityName: string, id: Guid): string ;
        beginDelete(entityName: string, id: Guid, callBack: (state: Object) => void): void ;
        endDelete(asyncState: Object): void ;
        update(entity: Entity): void ;
        beginUpdate(entity: Entity, callBack: (state: Object) => void): void ;
        endUpdate(asyncState: Object): void ;
        execute(request: OrganizationRequest): OrganizationResponse ;
        beginExecute(request: OrganizationRequest, callBack: (state: Object) => void): void ;
        endExecute(asyncState: Object): OrganizationResponse ;
    }

    var OrganizationServiceProxy: OrganizationServiceProxyFunc;

    export interface OptionSetValue {
        name: string;
        value: number;
    }
    export interface OptionSetValueFunc extends Function {
        prototype: OptionSetValue;
        new (value: number): OptionSetValue;
        parse(value: string): SparkleXrm.Sdk.OptionSetValue;
    }
    var OptionSetValue: OptionSetValueFunc;

    export interface Money {
        value: number;
    }
    export interface MoneyFunc extends Function {
        prototype: Money;
        new (value: number): Money;
    }
    var Money: MoneyFunc;

    export enum EntityStates {
        unchanged = 0,
        created = 1,
        changed = 2,
        deleted = 3,
        readOnly = 4
    }

    export enum EntityRole {
        referencing = 0,
        referenced = 1
    }

    export interface EntityReference {
        name: string;
        id: SparkleXrm.Sdk.Guid;
        logicalName: string;
        toString(): string;
    }
    export interface EntityReferenceFunc extends Function {
        prototype: EntityReference;
        new (Id: SparkleXrm.Sdk.Guid, LogicalName: string, Name: string): EntityReference;
    }
    var EntityReference: EntityReferenceFunc;

    export interface EntityCollection {
        entities: SparkleXrm.Sdk.DataCollectionOfEntity;
        entityName: string;
        getitem(index: number): SparkleXrm.Sdk.Entity;
        setitem(index: number, value: SparkleXrm.Sdk.Entity): void;
        minActiveRowVersion: string;
        moreRecords: boolean;
        pagingCookie: string;
        totalRecordCount: number;
        totalRecordCountLimitExceeded: boolean;
    }
    export interface EntityCollectionFunc extends Function {
        prototype: EntityCollection;
        new (entities: any): EntityCollection; //TODO:System.Collections.Generic.List$1<SparkleXrm.Sdk.Entity>): EntityCollection;
    }
    var EntityCollection: EntityCollectionFunc;

    export interface DateTimeEx {
    }
    export interface DateTimeExFunc extends Function {
        prototype: DateTimeEx;
        new (): DateTimeEx;
        toXrmString(date:Date): string;
        toXrmStringUTC(date: Date): string;
        padNumber(number: number, length: number): string;
        parse(dateString: string): Date;
        formatDuration(totalMinutes: number): string;
        parseDuration(duration: string): number;
        addTimeToDate(date: Date, time: string): Date;
        localTimeToUTC(LocalTime: Date, Bias: number, DaylightBias: number, DaylightYear: number, DaylightMonth: number, DaylightDay: number, DaylightHour: number, DaylightMinute: number, DaylightSecond: number, DaylightMilliseconds: number, DaylightWeekday: number, StandardBias: number, StandardYear: number, StandardMonth: number, StandardDay: number, StandardHour: number, StandardMinute: number, StandardSecond: number, StandardMilliseconds: number, StandardWeekday: number): Date;
        uTCToLocalTime(UTCTime: Date, Bias: number, DaylightBias: number, DaylightYear: number, DaylightMonth: number, DaylightDay: number, DaylightHour: number, DaylightMinute: number, DaylightSecond: number, DaylightMilliseconds: number, DaylightWeekday: number, StandardBias: number, StandardYear: number, StandardMonth: number, StandardDay: number, StandardHour: number, StandardMinute: number, StandardSecond: number, StandardMilliseconds: number, StandardWeekday: number): Date;
        getCutoverTime(CurrentTime: Date, Year: number, Month: number, Day: number, Hour: number, Minute: number, Second: number, Milliseconds: number, Weekday: number): Date;
        firstDayOfMonth(date: Date, Month: number): Date;
        dateAdd(interval: SparkleXrm.Sdk.DateInterval, value: number, date: Date): Date;
        firstDayOfWeek(date: Date): Date;
        lastDayOfWeek(date: Date): Date;
        formatTimeSpecific(dateValue: Date, formatString: string): string;
        formatDateSpecific(dateValue: Date, formatString: string): string;
        parseDateSpecific(dateValue: string, formatString: string): Date;
        setTime(date: Date, time: Date): void;
        setUTCTime(date: Date, time: Date): void;
        getTimeDuration(date: Date): number;
    }
    var DateTimeEx: DateTimeExFunc;

    const enum DateInterval {
        milliseconds,
        seconds,
        minutes,
        hours,
        days 
    }

    export interface DataCollectionOfEntity { //TODO:extends System.Collections.IEnumerable {

        get_item(i: number): SparkleXrm.Sdk.Entity;
        set_item(i: number, value: SparkleXrm.Sdk.Entity): void;
        get_count(): number;
        items():Entity[]; //TODO:System.Collections.Generic.List$1<SparkleXrm.Sdk.Entity>;
        //TODO:System$Collections$IEnumerable$getEnumerator(): System.Collections.IEnumerator;
    }
    export interface DataCollectionOfEntityFunc extends Function {
        prototype: DataCollectionOfEntity;
        //TODO:new (entities: System.Collections.Generic.List$1<SparkleXrm.Sdk.Entity>): DataCollectionOfEntity;
    }
    var DataCollectionOfEntity: DataCollectionOfEntityFunc;

    export interface AttributeTypes {
    }

    export interface AttributeTypesFunc extends Function {
        prototype: AttributeTypes;
        new (): AttributeTypes;
        string_: string;
        decimal_: string;
        int_: string;
        double_: string;
        dateTime_: string;
        boolean_: string;
        entityReference: string;
        guid_: string;
        optionSetValue: string;
        aliasedValue: string;
        entityCollection: string;
        money: string;
    }
    var AttributeTypes: AttributeTypesFunc;

    export interface Attribute {
        attributeName: string;
        typeName: string;
        formattedValue: string;
        value: any;
        id: string;
        logicalName: string;
        name: string;
    }
    export interface AttributeFunc extends Function {
        prototype: Attribute;
        new (attributeName: string, typeName: string): Attribute;
    }
    var Attribute: AttributeFunc;

}

declare module SparkleXrm.Sdk.Messages {
    export interface OrganizationRequest {
        serialise(): string;
       
    }

    /** @namespace SparkleXrm.Sdk.Messages */

    /**
     * Although depricated - this request is the only way to query the MultiEntitySearchEntities records
     *
     * @public
     * @class SparkleXrm.Sdk.Messages.ExecuteWorkflowRequest
     * @implements  SparkleXrm.Sdk.Messages.OrganizationRequest
     */
    export interface ExecuteWorkflowRequest extends SparkleXrm.Sdk.Messages.OrganizationRequest {
        entityId: string;
        workflowId: string;
    }
    export interface ExecuteWorkflowRequestFunc extends Function {
        prototype: ExecuteWorkflowRequest;
        new (): ExecuteWorkflowRequest;
    }
    var ExecuteWorkflowRequest: ExecuteWorkflowRequestFunc;

    export interface ExecuteWorkflowResponse  {
        id: string;
    }
    export interface ExecuteWorkflowResponseFunc extends Function {
        prototype: ExecuteWorkflowResponse;
        new (response: object): ExecuteWorkflowResponse;
    }
    var ExecuteWorkflowResponse: ExecuteWorkflowResponseFunc;


    
    
}
