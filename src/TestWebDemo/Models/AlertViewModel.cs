using System;
using TestWebDemo.Domain;

namespace TestWebDemo.Models
{
    public class AlertViewModel
    {
        public AlertViewModel() { }

        public AlertViewModel(Alert alert)
        {
            if (alert == null) throw new ArgumentNullException(nameof(alert));

            Id = alert.Id;
            Title = alert.Title;
            Content = alert.Content;
        }

        public string Id { get; set; }
        
        public string Title { get; set; }
        
        public string Content { get; set; } 
    }
}