﻿namespace Bennington.Cms.Buttons
{
    public class Button
    {
        public string Id { get; set; }
        public string Text { get; set; }
    }

    public class UrlButton : Button
    {
        public string Url { get; set; }
    }
}