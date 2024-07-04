using System.ComponentModel.DataAnnotations;

namespace toDoList.Models
{
    namespace toDoList.Models
    {
        public class W_TASK
        {
            [Key]
            public int C_ID_TASK { get; set; }
            public int C_ID_USER { get; set; }
            public string TITLE { get; set; } = string.Empty;
            public int PRIORITY { get; set; }
            public string DESCRIPTION { get; set; } = string.Empty;
            public string STATUS { get; set; } = string.Empty;
            public int STATE { get; set; } 
            public DateTime DATE_CONFIG { get; set; }
        }
    }

}
