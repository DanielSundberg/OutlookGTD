using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ComponentModel;

namespace OutlookGTDPlugin
{
    public class AnimalCategory
    {
        private string _category;
        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }

        private ObservableCollection<Animal> _animals;
        public ObservableCollection<Animal> Animals
        {
            get
            {
                if (_animals == null)
                    _animals = new ObservableCollection<Animal>();
                return _animals;
            }
        }

        public AnimalCategory()
        {
        }

        public AnimalCategory(
                    string category,
                    ObservableCollection<Animal> animals)
        {
            _category = category;
            _animals = animals;
        }
    }
    public class Animal : INotifyPropertyChanged
    {
        private string _name;
        private System.Windows.Visibility _visible = System.Windows.Visibility.Visible;

        public System.Windows.Visibility Visible
        {
            get { return _visible; }
            set 
            { 
                _visible = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Visible"));
                }
            }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public Animal()
        {
        }
        public Animal(string name)
        {
            _name = name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class TaskViewModel : INotifyPropertyChanged
    {
        ObservableCollection<AnimalCategory> _animalCategories ;
        ObservableCollection<Animal> _allAnimals = new ObservableCollection<Animal>();
        
        public TaskViewModel()
        {
            _animalCategories = new ObservableCollection<AnimalCategory>();
            ObservableCollection<Animal> animals = new ObservableCollection<Animal>();

            var a1 = new Animal("California Newt");
            animals.Add(a1);
            _allAnimals.Add(a1);
            var a2 = new Animal("Tomato Frog");
            animals.Add(a2);
            _allAnimals.Add(a2);
            var a3 = new Animal("Green Tree Frog");
            animals.Add(a3);
            _allAnimals.Add(a3);

            _animalCategories.Add(new AnimalCategory("Amphibians", animals));
            animals = new ObservableCollection<Animal>();
            var a5 = new Animal("Golden Silk Spider");
            animals.Add(a5);
            _allAnimals.Add(a5);
            var a4 = new Animal("Black Widow Spider");
            animals.Add(a4);
            _allAnimals.Add(a4);
            _animalCategories.Add(new AnimalCategory("Spiders", animals));
        }

        public ObservableCollection<AnimalCategory> Animals
        {
            get {
                return _animalCategories;
            }
        }

        public ObservableCollection<Animal> AllAnimals
        {
            get
            {
                return _allAnimals;
            }
        }

        public string SearchFilter 
        {
            get
            {
                return _searchFilter;
            }
            set
            {
                _searchFilter = value;
                if (string.IsNullOrEmpty(_searchFilter))
                {
                    //_allAnimals.ForEach(a => a.Visible = System.Windows.Visibility.Visible);
                    foreach (var a in _allAnimals)
                    {
                        a.Visible = System.Windows.Visibility.Visible;
                    }
                }
                else
                {
                    //_allAnimals.ForEach(a => a.Visible = (a.Name.ToLower().Contains(_searchFilter.ToLower()) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed));
                    foreach (var a in _allAnimals)
                    {
                        if (a.Name.ToLower().Contains(_searchFilter.ToLower()))
                            a.Visible = System.Windows.Visibility.Visible;
                        else
                            a.Visible = System.Windows.Visibility.Collapsed;
                    }
                }
                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private string _searchFilter;
    }
}
