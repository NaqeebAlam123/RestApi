using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoApi.Models
{
    [Table("tblBoard")]
    public class Board

    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<ToDo> ToDoList { get; set; } = new List<ToDo>();
    }
}
