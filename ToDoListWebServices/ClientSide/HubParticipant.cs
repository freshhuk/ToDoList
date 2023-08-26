namespace ToDoListWebServices.ClientSide
{
    public class HubParticipant
    {
        public string UserLogin { get; set; } // Идентификатор пользователя
        public string HubId { get; set; } // Идентификатор хаба, к которому участник присоединен
    }

}
