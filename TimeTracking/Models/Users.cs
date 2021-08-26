using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTracking.Models
{
    ///<summary>
    ///пользователи (модель)
    ///</summary>
    public class Users
    {
        [Key]
        public int id { get; set; }
        public string Email { get; set; }   
        
        [Required(ErrorMessage = "Введите фамилию")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        public string FirstName { get; set; }

        public string  PatroName { get; set; }

    }
}
