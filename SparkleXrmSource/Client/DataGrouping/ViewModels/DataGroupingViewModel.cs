// DataGroupingViewModel.cs
//

using jQueryApi;
using Slick;
using Slick.Data;
using Slick.Data.Aggregators;
using SparkleXrm;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm;

namespace Client.DataGrouping.ViewModels
{
    
    public class DataGroupingViewModel : ViewModelBase
    {
        public DataView Projects;
        public string SortCol;
        public int SortDir;
        //private Action<Func<object, object, int>, bool> _sortFunction;
        public DataGroupingViewModel()
        {
            DataViewOptions options = new DataViewOptions();
            options.GroupItemMetadataProvider = new GroupItemMetadataProvider();
            options.InlineFilters = true;
            Projects = new DataView(options);

            Projects.BeginUpdate();
            Projects.SetFilter(MyFilter);
            Dictionary<string, object> filterArguments = new Dictionary<string, object>(
                "percentComplete", 0);

            Projects.SetFilterArgs(filterArguments);
            LoadData();
            SetGrouping();
            Projects.EndUpdate();

        }
        public static bool MyFilter(object item, object args) {
            return true;//item["percentComplete"] >= args.percentComplete;
        }
        

        public void SetGrouping()
        {
            // Duration Grouping
            GroupingDefinition durationGrouping = new GroupingDefinition();
            durationGrouping.Getter = "duration";
            //durationGrouping.Formatter = delegate(DataViewGroup g)
            //{
            //    return "Duration:  " + g.Value + "  <span style='color:green'>(" + g.Count + " items)</span>";
              
            //};
            durationGrouping.Aggregators = new List<Aggregator>(
                new Sum("duration"),
                new Sum("cost"));
            durationGrouping.AggregateCollapsed = false;

            // Effort Grouping
            GroupingDefinition effortGrouping = new GroupingDefinition();
            effortGrouping.Getter = "effortDriven";
            //effortGrouping.Formatter = delegate(DataViewGroup g)
            //{
            //    return "Effort-Driven:  " + ((bool)g.Value ? "True" : "False") + "  <span style='color:green'>(" + g.Count + " items)</span>";
            //};
            effortGrouping.Aggregators = new List<Aggregator>(
                new Sum("duration"),
                new Sum("cost"));
            effortGrouping.Collapsed = false;

            // Percent Grouping
            GroupingDefinition percentGrouping = new GroupingDefinition();
            percentGrouping.Getter = "effortDriven";
            //percentGrouping.Formatter = delegate(DataViewGroup g)
            //{
            //    return "% Complete:  " + g.Value + "  <span style='color:green'>(" + g.Count + " items)</span>";
            //};
            percentGrouping.Aggregators = new List<Aggregator>(
                new Avg("percentComplete"));
            percentGrouping.Collapsed = false;

            Projects.SetGrouping(new List<GroupingDefinition>(durationGrouping, effortGrouping, percentGrouping));

           
        }
        public void LoadData()
        {
        
              List<string> someDates = new List<string>("01/01/2009", "02/02/2009", "03/03/2009");
              List<Project> data = new List<Project>();
              // prepare the data
              for (int i = 0; i < 100; i++) {
                Project d = new Project();
                ArrayEx.Add(data, d);

                d.id= "id_" + i;
                d.num = i;
                d.title = "Task " + i;
                d.duration = Math.Round(Math.Random() * 14);
                d.percentComplete = Math.Round(Math.Random() * 100);
                d.start = someDates[ Math.Floor((Math.Random()*2)) ];
                d.finish = someDates[ Math.Floor((Math.Random()*2)) ];
                d.cost = Math.Round(Math.Random() * 10000) / 100;
                d.effortDriven = (i % 5 == 0);
              }
              Projects.SetItems(data);

        }
    }

    public class Project
    {
        public string id;
        public int? num;
        public string title;
        public int? duration;
        public int? percentComplete;
        public string start;
        public string finish;
        public double? cost;
        public bool? effortDriven;
    }

   

    
}

