using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace AdminProject.Models
{
    public class MeasureListDto
    {
        public string MeasureName { get; set; }
        public List<ListItem> Items { get; set; }
    }
}