using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTracking.Models
{
    ///<summary>
    ///Отчет о работе за день внутри имеет Note Hour Date (модель)
    ///</summary>

    public class DailyWork
    {
        [Key]
        public int ID { get; set; }

        public int UserID { get; set; }
        [Required]

        public string Note { get; set; }

        [Required]
        public short Hour { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date{ get; set; }
}
}
