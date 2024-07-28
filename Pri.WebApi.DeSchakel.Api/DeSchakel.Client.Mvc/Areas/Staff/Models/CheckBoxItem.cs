using Microsoft.AspNetCore.Mvc;

namespace DeSchakel.Client.Mvc.Areas.Staff.Models
{
    public class CheckBoxItem
    {
        [HiddenInput]
        public int Value { get; set; }
        [HiddenInput]
        public string Text { get; set; }
        [HiddenInput]
        public bool IsSelected { get; set; }
    }
}
