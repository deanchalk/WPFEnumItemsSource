using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace EnumItemsSourceTutorial
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private CustomerType customerType;

        public MainViewModel()
        {
            ChangeCustomerTypeCommand =
                new DelegateCommand<CustomerType>(
                    OnChangeCustomerTypeCommand, IsNotProspect);
        }

        public CustomerType CustomerType
        {
            get => customerType;
            set
            {
                customerType = value;
                OnPropertyChanged();
                ChangeCustomerTypeCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand<CustomerType> ChangeCustomerTypeCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool IsNotProspect(CustomerType type)
        {
            return type != CustomerType.Prospect;
        }

        private void OnChangeCustomerTypeCommand(
            CustomerType type)
        {
            CustomerType = CustomerType.Prospect;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> action;
        private readonly Func<T, bool> canAction;

        public DelegateCommand(Action<T> action, Func<T, bool> canAction)
        {
            this.canAction = canAction;
            this.action = action;
        }

        public bool CanExecute(object parameter)
        {
            return parameter == null ? canAction(default(T)) : canAction((T)parameter);
        }

        public void Execute(object parameter)
        {
            action((T) parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}