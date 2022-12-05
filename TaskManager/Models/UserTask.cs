using Catel.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TaskManager.Models
{
    [Table("UserTasks")]
    public class UserTask
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public DateTime DueDate { get; set; } = DateTime.Today;
        public string Description { get; set; }

        //public Command FinishCommand { get; set; }

        public UserTask()
        {
            //FinishCommand = new Command(Finish);
        }

        private void Finish()
        {
            User.Tasks.Remove(this);
        }

        public override string ToString()
        {
            return $"{DueDate.ToShortDateString()} {Description}";
        }
    }
}
