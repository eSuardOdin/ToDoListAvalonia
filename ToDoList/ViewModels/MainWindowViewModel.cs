using System;
using System.Reactive.Linq;
using ReactiveUI;
using ToDoList.DataModel;
using ToDoList.Services;
namespace ToDoList.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        // A une dépendance au ToDoListService
        private ViewModelBase _contentViewModel; // View Model actuel
        public ToDoListViewModel ToDoList {  get; }
        
        // On affiche la todo list en priorité
        public MainWindowViewModel() 
        {
            var service = new ToDoListService();
            ToDoList = new ToDoListViewModel(service.GetItems());
            _contentViewModel = ToDoList;
        }

        // Changement du ViewModel
        public ViewModelBase ContentViewModel
        {
            get => _contentViewModel;
            private set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
        }

        // Changement pour AddItem
        public void AddItem()
        {
            AddItemViewModel addItemViewModel = new();

            Observable.Merge(
                addItemViewModel.OkCommand,
                addItemViewModel.CancelCommand.Select(_ => (ToDoItem?)null))
                .Take(1) // On ne prend en compte qu'un clic
                .Subscribe(newItem =>
                {
                    if (newItem != null)
                    {
                        ToDoList.ListItems.Add(newItem);
                        // Si bdd, on peut insert ici
                    }
                    ContentViewModel = ToDoList;
                });
            ContentViewModel = addItemViewModel;
        }


    }
}
