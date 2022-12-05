using Catel.MVVM;
using System;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
    public class UserTaskViewModel : ViewModelBase
    {
        [Model]
        public UserTask Task { get; set; }
        [ViewModelToModel]
        public int UserId { get; set; }
        [ViewModelToModel]
        public DateTime DueDate { get; set; }
        [ViewModelToModel]
        public string Description { get; set; }

        public UserTaskViewModel(UserTask task)
        {
            Task = task;
        }

        public override string Title { get { return "Задача"; } }

        // TODO: Register models with the vmpropmodel codesnippet
        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets
        // TODO: Register commands with the vmcommand or vmcommandwithcanexecute codesnippets

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
