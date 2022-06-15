using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApplication3Books.Entities

{
    [Table("Books")]
    public class BookEntity
    {

        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string author { get; set; }

        public string isbn { get; set; }


        public string type { get; set; }
    }
}
