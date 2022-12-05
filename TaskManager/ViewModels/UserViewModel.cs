using Catel.Collections;
using Catel.MVVM;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        [Model]
        public User User { get; set; }
        [ViewModelToModel]
        public string UserName { get; set; }
        [ViewModelToModel]
        public string Password { get; set; }

        public ObservableCollection<UserTaskViewModel> Tasks { get; set; } = new ObservableCollection<UserTaskViewModel>();
        public UserViewModel(User user)
        {
            User = user;
            Tasks.AddRange(user.Tasks.OrderBy(x=>x.DueDate).Select(x=> new UserTaskViewModel(x)).ToList());
        }

        public override string Title { get { return "Пользователь"; } }

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
