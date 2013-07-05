// IDataView.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Slick
{
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public interface IDataView 
    {

       PagingInfo GetPagingInfo();
       void SetPagingOptions(PagingInfo p);
       void Refresh();
       void InsertItem(int insertBefore, object item);
       void AddItem(object item);
       int GetLength();
      
      //        // methods
      //"beginUpdate": beginUpdate,
      //"endUpdate": endUpdate,
      //"setPagingOptions": setPagingOptions,
      //"getPagingInfo": getPagingInfo,
      //"getItems": getItems,
      //"setItems": setItems,
      //"setFilter": setFilter,
      //"sort": sort,
      //"fastSort": fastSort,
      //"reSort": reSort,
      //"groupBy": groupBy,
      //"setAggregators": setAggregators,
      //"collapseGroup": collapseGroup,
      //"expandGroup": expandGroup,
      //"getGroups": getGroups,
      //"getIdxById": getIdxById,
      //"getRowById": getRowById,
      //"getItemById": getItemById,
      //"getItemByIdx": getItemByIdx,
      //"mapRowsToIds": mapRowsToIds,
      //"mapIdsToRows": mapIdsToRows,
      //"setRefreshHints": setRefreshHints,
      //"setFilterArgs": setFilterArgs,
      //"refresh": refresh,
      //"updateItem": updateItem,
      //"insertItem": insertItem,
      //"addItem": addItem,
      //"deleteItem": deleteItem,
      //"syncGridSelection": syncGridSelection,
      //"syncGridCellCssStyles": syncGridCellCssStyles,

      //// data provider methods
      //"getLength": getLength,
      //"getItem": getItem,
      //"getItemMetadata": getItemMetadata,

      //// events
        //[IntrinsicProperty]
        //Event OnPagingInfoChanged { get; set; }
        //[IntrinsicProperty]
        //Event OnRowsChanged { get; set; }
        

    }
}
