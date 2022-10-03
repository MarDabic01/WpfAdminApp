using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfAdminApp.Model;

namespace WpfAdminApp.ViewModel
{
    public class NewEditWindowViewModel : INotifyPropertyChanged
    {
        private User currentPerson;
        private string windowTitle;
        private ICommand saveCommand;
        private Mediator mediator;

        public ICommand SaveCommand
        {
            get { return saveCommand; }
            set
            {
                if (saveCommand == value)
                {
                    return;
                }
                saveCommand = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SaveCommand"));
            }
        }
        public User CurrentPerson
        {
            get { return currentPerson; }
            set
            {
                if (currentPerson == value)
                {
                    return;
                }
                currentPerson = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentPerson"));
            }
        }
        public string WindowTitle
        {
            get { return windowTitle; }
            set
            {
                if (windowTitle == value)
                {
                    return;
                }
                windowTitle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowTitle"));
            }
        }
        public NewEditWindowViewModel(User person, Mediator mediator)
        {
            this.mediator = mediator;
            SaveCommand = new RelayCommand(SaveExecute, CanSave);
            CurrentPerson = person;
            WindowTitle = "Edit Person";
        }
        public NewEditWindowViewModel(Mediator mediator)
        {
            this.mediator = mediator;
            SaveCommand = new RelayCommand(SaveExecute, CanSave);
            CurrentPerson = new User();
            WindowTitle = "New Person";
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        void SaveExecute(object obj)
        {

            if (CurrentPerson != null && !currentPerson.HasErrors)
            {
                OnDone(new DoneEventArgs("Person saved", true));
                CurrentPerson.Save();
                mediator.Notify("PersonChange", CurrentPerson);
            }
            else
            {
                OnDone(new DoneEventArgs("Check your input", false));
            }
        }
        bool CanSave(object obj)
        {
            return true;
        }
        public delegate void DoneEventHandler(object sender, DoneEventArgs e);

        public class DoneEventArgs : EventArgs
        {
            private string message;

            public string Message
            {
                get { return message; }
                set
                {
                    if (message == value)
                    {
                        return;
                    }
                    message = value;
                }
            }

            private bool success;

            public bool Success
            {
                get { return success; }
                set { success = value; }
            }


            public DoneEventArgs(string message, bool success)
            {
                this.message = message;
                this.success = success;
            }
        }


        public event DoneEventHandler Done;

        public void OnDone(DoneEventArgs e)
        {
            if (Done != null)
            {
                Done(this, e);
            }
        }
    }
}
