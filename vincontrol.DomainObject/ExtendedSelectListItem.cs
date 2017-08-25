using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vincontrol.DomainObject
{
    public class SelectListItem
    {
        public SelectListItem() { }

        public SelectListItem(int value, string text, bool selected) 
        {
            Value = value.ToString();
            Text = text;
            Selected = selected;
        }

        public SelectListItem(string value, string text, bool selected)
        {
            Value = value;
            Text = text;
            Selected = selected;
        }

        public string Value { get; set; }
        public string Text { get; set; }
        public bool Selected { get; set; }
        public int SortOrder { get; set; }
    }

    public class ExtendedSelectListItem
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public bool Selected { get; set; }
        public int SortOrder { get; set; }
    }

    public class ExtendedSelectDetailListItem : ExtendedSelectListItem
    {
        public string Description { get; set; }
    }

    public class SelectListItemTemplates : SelectListItem
    {
        public SelectListItemTemplates(string value, string text, bool selected, int departmentId)
        {
            Value = value;
            Text = text;
            Selected = selected;
            DepartmentId = departmentId;
        }
        public int DepartmentId { get; set; }
    }
}
