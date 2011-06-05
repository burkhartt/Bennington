﻿using System.Collections.Generic;

namespace SampleApp.Models
{
    public class ListPageViewModel
    {
        public IEnumerable<object> Items { get; set; }
    }

    public class LocationViewModel
    {
        public string State { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
    }
}