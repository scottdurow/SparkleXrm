// TreeDataView.cs
//

using Slick;
using SparkleXrm;
using SparkleXrm.GridEditor;
using System;
using System.Collections.Generic;
using Xrm.Sdk;

namespace Client.DataGrouping.ViewModels
{
    public class TreeDataViewModel : ViewModelBase
    {
        public TreeDataView Items;
        public TreeDataViewModel()
        {
            Items = new TreeDataView();
            // Create some data
            Items.BeginUpdate();

            // Add items
            int idCounter =1;

            GroupedItems group1 = CreateGroup("Project 1", idCounter.ToString());
            group1.ChildItems = new List<object>();
            for (int i = 2; i < 100; i++)
            {
                idCounter++;
                GroupedItems group2 = new GroupedItems();
                group2.Title = "Project " + idCounter.ToString();
              
                group2.Id = new Guid(idCounter.ToString());
                group2.ChildItems = new List<object>();

                // Add Child Items
                for (int j = 0; j < 25; j++)
                {
                    idCounter++;
                    AddItem(group2, "Item " + idCounter.ToString());
                }
                group1.ChildItems.Add(group2);
            }
            

            Items.AddItem(group1);

            Items.EndUpdate();
        }

        private static GroupedItems CreateGroup(string projectName, string id)
        {
            GroupedItems group1 = new GroupedItems();
            group1.Title = projectName;
            group1.ChildItems = new List<object>();
            group1.Id = new Guid(id);
            return group1;
        }

        private static void AddItem(GroupedItems group2,string title)
        {
            TreeItem item1 = new TreeItem();
            item1.Id = new Guid("3");
            item1.status = "1";
            item1.project = title;
            group2.ChildItems.Add(item1);
        }
    }

    public class TreeItem
    {
        public Guid Id;
        public string status;
        public string project;
        public string date;
        public string employee;
        public string duration;
        public List<TreeItem> Items;
    }
    public class GroupedItems
    {
        public Guid Id;
        public string Title;
       
        public List<object> ChildItems;

  
    }
    
}
