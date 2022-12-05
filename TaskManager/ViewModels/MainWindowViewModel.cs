using Catel.MVVM;
using Catel.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public TaskCommand NewtaskCommand { get; set;}
        public TaskCommand EdittaskCommand { get; set; }
        public TaskCommand FinishtaskCommand { get; set; }

        private IUIVisualizerService _visualizerService;
        public UserTaskViewModel SelectedTask { get; set; }
        public UserViewModel CurrentUser { get; set; }

        private ApplicationContext _context;
        private IMessageService _messageService;
        private int _counter { get; set; } = 0;
        public MainWindowViewModel(IMessageService messageService, IUIVisualizerService visualizerService)
        {
            NewtaskCommand = new TaskCommand(Newtask);
            EdittaskCommand = new TaskCommand(Edittask, Canedit);
            FinishtaskCommand = new TaskCommand(Finishtask, Canedit);
            _messageService = messageService;
            _visualizerService= visualizerService;
        }

        private bool Canedit()
        {
            return SelectedTask != null;
        }

        private async Task Finishtask()
        {
            try
            {
                if (await _messageService.ShowAsync("Удалить задачу?", "Удаление", MessageButton.YesNo, MessageImage.Question) == MessageResult.Yes)
                {
                    CurrentUser.User.Tasks.Remove(SelectedTask.Task);
                    CurrentUser.Tasks.Remove(SelectedTask);
                    _context.Users.Update(CurrentUser.User);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                await _messageService.ShowErrorAsync(e.ToString());
            }
        }
        private async Task Edittask()
        {
            try
            {
                var rv = await _visualizerService.ShowDialogAsync(SelectedTask);
                if(rv == true)
                {
                    _context.UsersTasks.Update(SelectedTask.Task);
                    await _context.SaveChangesAsync();
                }
            }
            catch(Exception e)
            {
                await _messageService.ShowErrorAsync(e.ToString());
            }
        }

        private async Task Newtask()
        {
            try
            {
                
                var task = new UserTask {  User = CurrentUser.User, UserId = CurrentUser.User.Id };
                var vm = new UserTaskViewModel(task);
                var rv = await _visualizerService.ShowDialogAsync(vm);
                if (rv == true)
                {
                    CurrentUser.User.Tasks.Add(task);
                    CurrentUser.Tasks.Add(new UserTaskViewModel(task));
                    _context.Users.Update(CurrentUser.User);
                    await _context.SaveChangesAsync();
                }
                
            }
            catch (Exception e)
            {
                await _messageService.ShowErrorAsync(e.ToString());
            }
        }

        public override string Title { get { return "Менеджер задач"; } }

        // TODO: Register models with the vmpropmodel codesnippet
        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets
        // TODO: Register commands with the vmcommand or vmcommandwithcanexecute codesnippets

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            try
            {
                _context = new ApplicationContext();
                _context.Database.EnsureCreated();

            retry:
                var vm = new LoginViewModel();
                var rv = await _visualizerService.ShowDialogAsync(vm);
                if (rv != true)
                {
                    App.Current.Shutdown();
                }
                else
                {
                    if (vm.IsNewUser)
                    {
                        var user = _context.Users.FirstOrDefault(x=>x.UserName.ToLower() == vm.UserName.ToLower());
                        if (user != null)
                        {
                            await _messageService.ShowErrorAsync($"Пользователь уже существует!");
                            goto retry;
                        }
                        else
                        {
                            _context.Users.Add(new User { UserName = vm.UserName, Password = vm.Password });
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        var user = _context.Users.FirstOrDefault(x => x.UserName == vm.UserName && x.Password == vm.Password);

                        if (user == null)
                        {

                            await _messageService.ShowInformationAsync($"Не верное имя пользователя или пароль!");
                            goto retry;
                        }
                        else
                        {
                            CurrentUser = new UserViewModel(user);
                            await _messageService.ShowInformationAsync($"Привет, {CurrentUser.UserName}!");
                        }
                    }
                }

            }
            catch(Exception e)
            {
                await _messageService.ShowErrorAsync(e.ToString());
            }
            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
