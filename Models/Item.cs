namespace ToDo.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string GroupToDo { get; set; }
        public string TextToDo { get; set; }
        public bool CheckedDone { get; set; } = false;
    }
}
