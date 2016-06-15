using System;
using TestWebDemo.Services;

namespace TestWebDemo.Domain
{
    public class Alert : IAggregateRoot
    {
        private readonly IClock _clock;

        public Alert(IClock clock)
        {
            _clock = clock;
        }

        public string Id { get; }

        public string Title { get; private set; }

        public string Content { get; private set; }

        public Alert SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentNullException(nameof(title));

            Title = title;

            return this;
        }
        public Alert SetContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) throw new ArgumentNullException(nameof(content));

            Content = content;

            return this;
        }
    }
}