using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
#nullable enable
namespace ToDoApi.Models
{
    [Table("tblToDo")]
    public class ToDo
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public bool Done { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public int BoardId { get; set; }

        public Board? Board { get; set; }
    }
}
