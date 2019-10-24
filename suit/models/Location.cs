using System;
using System.Collections.Generic;
using System.Text;

namespace suit
{ 
    public class Location
    {
        public String Address { get; set; }
        public String ID { get; set; }
        public String Neighborhood { get; set; }
        public String PDV { get; set; }
        public String Type { get; set; }
        public List<Tasks> Tasks { get; set; }
    }
}
