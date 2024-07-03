using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace toDoList.Models
{
    public class W_USER
    {
        [Key]
        public int C_ID_USER { get; set; }
        public string USERNAME { get; set; } = string.Empty;
        public string PASSWORD { get; set; } = string.Empty;
    }
}
