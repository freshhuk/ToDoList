namespace ToDoListWebDomain.Domain.Models
{
    public class ChangeTaskModel
    {
        //Айди задачи которую мы хотим изменить
        public int Id { get; set; }
        //Новые значение полей для задачи
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public DateTime TaskData { get; set; }
        public string TaskStatus { get; set; }
    }
}
