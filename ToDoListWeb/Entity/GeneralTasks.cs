using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ToDoListWeb.Entity
{
    public class GeneralTasks
    {
        public int Id { get; set; }
        public string NameTask { get; set; }
        public string DescriptionTask { get; set; }
        public string Status { get; set; }
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime TaskTime { get; set; }
        public string UserName { get; set; }
    }
}
