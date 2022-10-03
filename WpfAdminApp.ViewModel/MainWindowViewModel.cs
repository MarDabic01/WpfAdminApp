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
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private User currentPerson;
        private UserCollection personList;
        private Mediator mediator;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
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

        public UserCollection PersonList
        {
            get { return personList; }
            set
            {
                if (personList == value)
                {
                    return;
                }
                personList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PersonList"));
            }
        }
        public MainWindowViewModel(Mediator mediator)
        {
            this.mediator = mediator;
            mediator.Register("PersonChange", PersonChanged);
            PersonList = UserCollection.GetAllPersons();
            CurrentPerson = new User();
        }
        void PersonChanged(object sender)
        {
            User person = (User)sender;

            int index = PersonList.IndexOf(person);

            if (index != -1)
            {
                PersonList.RemoveAt(index);
                PersonList.Insert(index, person);
            }
            else
            {
                PersonList.Add(person);
            }
        }
    }
}
