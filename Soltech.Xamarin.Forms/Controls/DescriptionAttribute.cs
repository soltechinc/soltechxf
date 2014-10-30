using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolTech.Forms
{
    public class DescriptionAttribute : Attribute
    {
        public String Description { get; set; }
        public String ResourceId { get; set; }

        public DescriptionAttribute(String resourceId)
        {
            if (String.IsNullOrEmpty(resourceId)) throw new ArgumentNullException("resourceId");
            ResourceId = resourceId;
        }
    }
}
