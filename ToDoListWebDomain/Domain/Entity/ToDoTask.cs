using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoListWebDomain.Domain.Entity
{
    public class ToDoTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string NameTask { get; set; }
        public string DescriptionTask { get; set; }
        public string Status { get; set; }
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime TaskTime { get; set; }

    }
}
