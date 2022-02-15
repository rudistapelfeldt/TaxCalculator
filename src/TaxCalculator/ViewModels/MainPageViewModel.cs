using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using MvvmHelpers;
using Prism.AppModel;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using TaxCalculator.Models;
using TaxCalculator.Strings;

namespace TaxCalculator.ViewModels
{
    public class MainPageViewModel : ViewModelBase      
    {

        #region Private Members
        string _answer;
        #endregion

        #region Public members
        public string Input { get; set; }
        public string Answer
        {
            get => _answer;
            set
            {
                SetProperty(ref _answer, value);
                OnPropertyChanged("Answer");
            }
        }
        #endregion

        public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, 
                                 IDeviceService deviceService)
            : base(navigationService, pageDialogService, deviceService)
        {
            Title = "TaxCalculator";
            TodoItems = new ObservableRangeCollection<TodoItem>();
            
        }

        public ObservableRangeCollection<TodoItem> TodoItems { get; set; }

        public DelegateCommand CalculateCommand => new DelegateCommand(Calculate);

        public DelegateCommand<TodoItem> DeleteItemCommand { get; }

        public DelegateCommand<TodoItem> TodoItemTappedCommand { get; }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            IsBusy = true;
            switch(parameters.GetNavigationMode())
            {
                case NavigationMode.Back:
                    if(parameters.ContainsKey("todoItem"))
                    {
                        TodoItems.Add(parameters.GetValue<TodoItem>("todoItem"));
                    }
                    break;
                case NavigationMode.New:
                    TodoItems.AddRange(parameters.GetValues<string>("todo")
                                         .Select(n => new TodoItem { Name = n }));
                    break;
            }
            IsBusy = false;
        }

        public void Calculate()
        {
            try
            {
                if (!Validate())
                    return;
                Answer = "0";
                int percentage;
                switch (Int32.Parse(Input))
                {
                    case int n when (n < 189880):
                        percentage = 18;
                        break;
                    case int n when (n >= 189881 && n <= 296540):
                        percentage = 26;
                        break;
                    case int n when (n >= 296541 && n <= 410460):
                        percentage = 31;
                        break;
                    case int n when (n >= 410461 && n <= 555600):
                        percentage = 36;
                        break;
                    case int n when (n >= 555601 && n <= 555600):
                        percentage = 39;
                        break;
                    case int n when (n <= 708311 && n >= 1500000):
                        percentage = 41;
                        break;
                    default:
                        percentage = 45;
                        break;


                }

                Answer = Convert.ToString((double.Parse(Input) * percentage) / 100);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            
        }

        public bool Validate()
        {
            if (string.IsNullOrEmpty(Input))
            {
                _pageDialogService.DisplayAlertAsync("Attention", "Please enter salary", "Ok");
                return false;
            }
            else
            {
                return true;
            }
                 
        }

        private async void OnAddItemCommandExecuted() => 
            await _navigationService.NavigateAsync("TodoItemDetail", new NavigationParameters
            {
                { "new", true },
                { "todoItem", new TodoItem() }
            });

        private void OnDeleteItemCommandExecuted(TodoItem item) =>
            TodoItems.Remove(item);

        private async void OnTodoItemTappedCommandExecuted(TodoItem item) =>
            await _navigationService.NavigateAsync("TodoItemDetail", new NavigationParameters{
                { "todoItem", item }
            });

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);
        }
    }
}
