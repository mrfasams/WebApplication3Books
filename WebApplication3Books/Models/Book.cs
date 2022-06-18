namespace WebApplication3Books.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string author { get; set; }

        public string isbn { get; set; }

        public string description { get; set; }
        public string type { get; set; }

        public byte[] Content { get; set; }

        public string FileType { get; set; }
        public string Extension { get; set; }

        public string fileName { get; set; }


    }
}
