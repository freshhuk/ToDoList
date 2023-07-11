namespace ToDoListWeb.Enums
{
    public class SortTaskEnum
    {
        public enum SortTaskType 
        {
            SortTaskDateFromMinToMax = 0,
            SortTaskDateFromMaxToMin = 1,
            SortRecentlyAdded = 2,
            SortOldAdded = 3,
            NoSort = 4, 
        }
    }
}
