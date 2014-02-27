

using System.Collections.Generic;

public class SiteMap 
    {
        public List<Area> areas = new List<Area>();
        public string url;
        public List<string> privileges;
 
    }
public class SiteMapElement
{
    public string title;
}
public class Group : SiteMapElement
    {
        public string id;
        public string icon;
        public List<SubArea> subareas;
    

    }
public class Area : SiteMapElement
    {
        public List<Group> groups = new List<Group>();
        public string id;
        public string icon;
       
    }
public class SubArea : SiteMapElement
    {
        public string id;
        public string icon;
        public string entity;
        public int objecttypecode;
        public string url;
        public List<string> privileges;
        
    }

   
