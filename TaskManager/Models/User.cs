using Catel.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TaskManager.Models
{
    /// <summary>
    /// Класс модели пользователя
    /// </summary>
    [Table("Users")]
    public class User 
    {
        public int Id { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; }

        public List<UserTask> Tasks { get; private set; } = new List<UserTask>();
        public override string ToString()
        {
            return UserName;
        }
    }
}
