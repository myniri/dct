namespace BLL.DTOs
{
    public class HackerNewsStoryDTO
    {
        public string Title { get; set; }

        public string Uri { get; set; }

        public string PostedBy { get; set; }

        public DateTimeOffset Time { get; set; }

        public uint Score { get; set; }

        public uint CommentCount { get; set; }
    }
}