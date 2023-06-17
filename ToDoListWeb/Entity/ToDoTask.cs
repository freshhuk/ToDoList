using System.ComponentModel.DataAnnotations;

namespace ToDoListWeb.Entity
{
    public class ToDoTask
    {
        public int Id { get; set; }
        public string NameTask { get; set; }
        public string DescriptionTask { get; set; }
        public string Status { get; set; }
        
        public DateTime TaskTime { get; set; }

    }
}
