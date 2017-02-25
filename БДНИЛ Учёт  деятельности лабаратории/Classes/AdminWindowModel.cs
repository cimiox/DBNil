using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using БДНИЛ_Учёт__деятельности_лабаратории.addition;

namespace БДНИЛ_Учёт__деятельности_лабаратории.Classes
{
    public class AdminWindowModel : Notifier
    {
        //Вызов коман и методов Модели
        public AdminWindowModel()
        {
            EditCmd = new Command(EditCmdExecute, EditCmdCanExecute);
            AddCmd = new Command(AddCmdExecute, AddCmdCanExecute);

            EditCmdPlants = new Command(EditCmdExecutePlants, EditCmdCanExecutePlants);
            AddCmdPlants = new Command(AddCmdExecutePlants, AddCmdCanExecutePlants);

            LoadUsers();
            LoadTitle();
            LoadPlants();

            CreateUsersForCreate();
            CreatePlantsForCreate();
        }



        //Создание коллекции для сущности UserRoles
        private List<UserRoles> _userRoles;
        public List<UserRoles> UserRoles {
            get { return _userRoles; }
            set {
                _userRoles = value;
                OnPropertyChanged("UserRoles");
            }
        }

        //Создание коллекции для сущности User
        private List<Users> _user;
        public List<Users> User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged("User"); }
        }

        //Создание коллекции, чтобы заполнить записями из сущности combobox
        private List<UserRoles> _title;
        public List<UserRoles> Title {
            get { return _title; }
            set {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        //Загрузка Коллекции Title
        private void LoadTitle() {
            Task.Run(() => {
                try
                {
                    using (var db = new Entities())
                    {
                        Title = db.UserRoles.ToList();
                    }

                }
                catch (Exception e)
                {
                    MessageBox.Show("Ошибка загрузки Ролей!");
                }
            });
        }

        //Получение данных которые были выбраны для редактирования
        private UserRoles _selectedEditTitle;
        public UserRoles SelectedEditTitle {
            get { return _selectedEditTitle; }
            set {
                _selectedEditTitle = value;
                OnPropertyChanged("SelectedEditTitle");
            }
        }

        //реализация команд редактирования и добавления
        public ICommand EditCmd { get; set; }
        public ICommand AddCmd { get; set; }

        //Создание коллекции динамических данных
        private ObservableCollection<UsersItemWrapper> _users;
        public ObservableCollection<UsersItemWrapper> Users {
            get { return _users; }
            set {
                _users = value;
                OnPropertyChanged("Users");
            }
        }

        //Запись данных которые буду редактироваться
        private Users _editUsers;
        public Users EditUsers
        {
            get { return _editUsers; }
            set
            {
                _editUsers = value;
                OnPropertyChanged("EditUsers");
            }
        }

        //Получение данных новой записи для базы данных
        private Users _newUsers;
        public Users NewUsers
        {
            get { return _newUsers; }
            set
            {
                _newUsers = value;
                OnPropertyChanged("NewUsers");
            }
        }

        //Выбор нового Title
        private UserRoles _selectedNewTitle;
        public UserRoles SelectedNewTitle
        {
            get { return _selectedNewTitle; }
            set
            {
                _selectedNewTitle = value;
                OnPropertyChanged("SelectedNewTitle");
            }
        }

        //Загрузка коллекции Users в Datagrid
        private void LoadUsers() {
            Task.Run(() =>
            {
                try {
                    using (var db = new Entities()) {
                        var items = db.Users.Include("UserRoles").Where(i => i.UserRoleId == i.UserRoles.IdUserRole).ToList();

                        List<UsersItemWrapper> result = new List<UsersItemWrapper>();
                        foreach (var item in items)
                        {
                            UsersItemWrapper wrp = new UsersItemWrapper();
                            wrp.Users = item;

                            wrp.RemoveCmd = new Command(() => { RemoveItemCmdCanExecute(wrp); });
                            wrp.EditCmd = new Command(() => { PreparingEditWrapper(item); });

                            result.Add(wrp);
                        }
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Users = new ObservableCollection<UsersItemWrapper>(result);
                            Users.CollectionChanged += Users_CollectionChanged;
                        });
                    }
                }
                catch(Exception e) {
                    MessageBox.Show("Ошибка загрузки Пользователей!");
                }
            });
        }
        
        //Обертка для изменения
        private void PreparingEditWrapper(Users item) {
            Users _new = new Users();
            _new.IdUsers = item.IdUsers;
            _new.Login = item.Login;
            _new.Password = null;
            _new.FName = item.FName;
            _new.LName = item.LName;
            _new.UserRoleId = item.UserRoles.IdUserRole;
            _new.UserPatronymic = item.UserPatronymic;
            UserRoles _newRoles = new UserRoles();
            _newRoles.IdUserRole = item.UserRoles.IdUserRole;
            _newRoles.Title = item.UserRoles.Title;


            EditUsers = _new;

            SelectedEditTitle = Title.SingleOrDefault(i => i.IdUserRole == _new.UserRoleId);
        }

        //Проверка на заполнения полей
        private bool EditCmdCanExecute()
        {
            try {
                if (EditUsers == null) return false;
                if (SelectedEditTitle == null) return false;

                if (EditUsers.Login.Trim() == string.Empty) return false;
                if (EditUsers.FName.Trim() == string.Empty) return false;
                if (EditUsers.LName.Trim() == string.Empty) return false;
                if (EditUsers.UserPatronymic.Trim() == string.Empty) return false;
                return true;
            }
            catch {   return false;  }
        }

        //Описание команды для редактирования
        private void EditCmdExecute()
        {
            Task.Run(() =>
            {
                using (Entities db = new Entities()) {
                    var transaction = db.Database.BeginTransaction();
                    try
                    {
                        EditUsers.UserRoles = SelectedEditTitle;
                        EditUsers.UserRoleId = SelectedEditTitle.IdUserRole;

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch {
                        transaction.Rollback();
                        MessageBox.Show("Ошибка изменения Пользователя");
                        return;
                    }
                }

                try
                {
                    UsersItemWrapper wrapper = Users.SingleOrDefault(i => i.Users.IdUsers == EditUsers.IdUsers);
                    UsersItemWrapper _newWrapper = new UsersItemWrapper();
                    _newWrapper.Users = EditUsers;

                    _newWrapper.RemoveCmd = new Command(() => { RemoveItemCmdCanExecute(_newWrapper); });
                    _newWrapper.EditCmd = new Command(() => { PreparingEditWrapper(_newWrapper.Users); });

                    int index = Users.IndexOf(wrapper);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Users.Remove(wrapper);
                        Users.Insert(index, _newWrapper);

                        EditUsers = null;
                        SelectedEditTitle = null;
                    });
                }
                catch { MessageBox.Show("Ошибка поиска Пользователя"); };
            });
        }

        //
        private void Users_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        //Удаление выбранного элемента
        private void RemoveItemCmdCanExecute(UsersItemWrapper item)
        {
            Task.Run(() => {
                try {
                    using (Entities db = new Entities()) {
                        db.Users.Remove(db.Users.Find(item.Users.IdUsers));
                        db.SaveChanges();
                    }
                    Application.Current.Dispatcher.Invoke(() => {
                        Users.Remove(item);
                    });
                }
                catch {
                    MessageBox.Show("Ошибка удаления Пользователя");
                }
            });
        }

        //Создание сущности Users для создания новой сущности
        private void CreateUsersForCreate()
        {
            NewUsers = new Users()
            {
                UserRoles = new UserRoles()
            };
        }

        //Проверка на заполнение полей
        private bool AddCmdCanExecute()
        {
            try
            {
                if (NewUsers.Login.Trim() == string.Empty) return false;
                if (NewUsers.FName.Trim() == string.Empty) return false;
                if (NewUsers.LName.Trim() == string.Empty) return false;
                if (NewUsers.UserPatronymic.Trim() == string.Empty) return false;

                if (SelectedNewTitle == null) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Описание команды добавления
        private void AddCmdExecute()
        {
            Task.Run(() => {

                var db = new Entities();
                var transaction = db.Database.BeginTransaction();

                try
                {
                    Users _new = new Users() { Login = NewUsers.Login.Trim(), Password = null, FName = NewUsers.FName.Trim(), LName = NewUsers.LName.Trim(), UserPatronymic = NewUsers.UserPatronymic.Trim(), BanMessageId = null, UserRoleId = SelectedNewTitle.IdUserRole };
                    db.Users.Add(_new);
                    db.SaveChanges();

                    transaction.Commit();

                    SelectedNewTitle = null;

                    UsersItemWrapper item = new UsersItemWrapper()
                    {
                        Users = _new
                    };
                    item.RemoveCmd = new Command(() => { RemoveItemCmdCanExecute(item); }, null);
                    item.EditCmd = new Command(() => { PreparingEditWrapper(_new); }, null);

                    Application.Current.Dispatcher.Invoke(() => {
                        Users.Add(item);
                        CreateUsersForCreate();
                    });


                }
                catch
                {
                    transaction.Rollback();
                    MessageBox.Show("Add error!");
                }
            });
        }



        //Model Plants

        //Описание команд добавления и редактирования
        public ICommand EditCmdPlants { get; set; }
        public ICommand AddCmdPlants { get; set; }

        //Создание коллекции для сущности Plant
        private List<Plants> _plant;
        public List<Plants> Plant { get { return _plant; }
            set
            {
                _plant = value;
                OnPropertyChanged("Plant");
            }
        }

        //Создание новой записи в сущности Plant
        private Plants _newPlants;
        public Plants NewPlants { get { return _newPlants; }
            set
            {
                _newPlants = value;
                OnPropertyChanged("NewPlants");
            }
        }

        //Уведомление об редактировании сущности
        private Plants _editPlants;
        public Plants EditPlants
        {
            get { return _editPlants; }
            set
            {
                _editPlants = value;
                OnPropertyChanged("EditPlants");
            }
        }

        //Создание коллекции динамических данных
        private ObservableCollection<PlantsItemWrapper> _plants;
        public ObservableCollection<PlantsItemWrapper> Plants
        { get { return _plants; }
            set
            {
                _plants = value;
                OnPropertyChanged("Plants");
            }
        }

        //Загрузка в DataGrid коллекции Plants
        private void LoadPlants() {
            Task.Run(() => {
                try {
                    using (Entities db = new Entities()) {
                        var items = db.Plants.Include("Employee").Where(i => i.EmployeeId == i.Employee.IdEmployee).ToList();

                        List<PlantsItemWrapper> result = new List<PlantsItemWrapper>();
                        foreach (var item in items)
                        {
                            PlantsItemWrapper wrp = new PlantsItemWrapper();
                            wrp.Plants = item;

                            wrp.RemoveCmdPlants = new Command(() => { RemovePlantsItemCmdCanExecute(wrp); });
                            wrp.EditCmdPlants = new Command(() => { PreparingEditWrapperPlants(item); });

                            result.Add(wrp);
                        }
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Plants = new ObservableCollection<PlantsItemWrapper>(result);
                            Plants.CollectionChanged += Plants_CollectionChanged;
                        });
                    }
                }
                catch {
                    MessageBox.Show("Ошибка загрузки Сотрудников");
                }
            });
        }

        //Удаление выбранной записи
        private void RemovePlantsItemCmdCanExecute(PlantsItemWrapper item)
        {
            Task.Run(() => {
                try
                {
                    using (Entities db = new Entities())
                    {
                        db.Plants.Remove(db.Plants.Find(item.Plants.IdPlant));
                        db.SaveChanges();
                    }
                    Application.Current.Dispatcher.Invoke(() => {
                        Plants.Remove(item);
                    });
                }
                catch
                {
                    MessageBox.Show("Ошибка удаления");
                }
            });
        }

        private void Plants_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }

        //обертка для сущности Plants
        private void PreparingEditWrapperPlants(Plants item)
        {
            Plants _newPlants = new Plants();
            _newPlants.IdPlant = item.IdPlant;
            _newPlants.Count = item.Count;
            _newPlants.Age = item.Age;
            _newPlants.Sort = item.Sort;
            _newPlants.PrimeCost = item.PrimeCost;
            _newPlants.EmployeeId = item.EmployeeId;
            Employee _newEmployee = new Employee();
            _newEmployee.IdEmployee = item.Employee.IdEmployee;
            _newEmployee.Name = item.Employee.Name;
            _newEmployee.Surname = item.Employee.Surname;
            _newEmployee.Patronymic = item.Employee.Patronymic;
            _newEmployee.WorkExpirience = item.Employee.WorkExpirience;
            _newEmployee.DayPerfomance = item.Employee.DayPerfomance;
            _newPlants.Employee = _newEmployee;

            EditPlants = _newPlants;
        }

        //Создание записи для создания новой
        public void CreatePlantsForCreate() {
            NewPlants = new Plants()
            {
                Employee = new Employee()
            };
        }

        //Проверка на заполнение полей
        private bool AddCmdCanExecutePlants()
        {
            try
            {
                if (NewPlants.Employee.Surname.Trim() == string.Empty) return false;
                if (NewPlants.Employee.Name.Trim() == string.Empty) return false;
                if (NewPlants.Employee.Patronymic.Trim() == string.Empty) return false;
                if (NewPlants.Employee.WorkExpirience == string.Empty) return false;
                if (NewPlants.Sort.Trim() == string.Empty) return false;
                if (NewPlants.Age.Trim() == string.Empty) return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        //добавление новой записи
        private void AddCmdExecutePlants()
        {
            Task.Run(() => {

                var db = new Entities();
                var transaction = db.Database.BeginTransaction();

                try
                {
                    Employee _newEmployee = new Employee() { Name = NewPlants.Employee.Name.Trim(), Surname = NewPlants.Employee.Surname.Trim(), Patronymic = NewPlants.Employee.Patronymic.Trim(), WorkExpirience = NewPlants.Employee.WorkExpirience, DayPerfomance = NewPlants.Employee.DayPerfomance };
                    db.Employee.Add(_newEmployee);
                    db.SaveChanges();

                    Plants _new = new Plants() { Sort = NewPlants.Sort.Trim(), Count = NewPlants.Count, Age = NewPlants.Age.Trim(), EmployeeId = _newEmployee.IdEmployee};
                    db.Plants.Add(_new);
                    db.SaveChanges();

                    transaction.Commit();

                    PlantsItemWrapper item = new PlantsItemWrapper()
                    {
                        Plants = _new
                    };
                    item.RemoveCmdPlants = new Command(() => { RemovePlantsItemCmdCanExecute(item); }, null);
                    item.EditCmdPlants = new Command(() => { PreparingEditWrapperPlants(_new); }, null);

                    Application.Current.Dispatcher.Invoke(() => {
                        Plants.Add(item);
                        CreatePlantsForCreate();
                    });


                }
                catch(Exception e)
                {
                    transaction.Rollback();
                    MessageBox.Show("Add error!");
                }
            });
        }

        //Описание команды на редактирование
        private void EditCmdExecutePlants()
        {
            Task.Run(() =>
            {
                using (Entities db = new Entities())
                {
                    var transaction = db.Database.BeginTransaction();
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        MessageBox.Show("Ошибка изменения Пользователя");
                        return;
                    }
                }

                try
                {
                    PlantsItemWrapper wrapper = Plants.SingleOrDefault(i => i.Plants.IdPlant == EditPlants.IdPlant);
                    PlantsItemWrapper _newWrapper = new PlantsItemWrapper();
                    _newWrapper.Plants = EditPlants;

                    _newWrapper.RemoveCmdPlants = new Command(() => { RemovePlantsItemCmdCanExecute(_newWrapper); });
                    _newWrapper.EditCmdPlants = new Command(() => { PreparingEditWrapperPlants(_newWrapper.Plants); });

                    int index = Plants.IndexOf(wrapper);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Plants.Remove(wrapper);
                        Plants.Insert(index, _newWrapper);

                        EditPlants = null;
                    });
                }
                catch { MessageBox.Show("Ошибка поиска Пользователя"); };
            });
        }

        //проверка на заполнение полей
        private bool EditCmdCanExecutePlants()
        {
            try
            {
                if (EditPlants == null) return false;
                if (EditPlants.Employee.Surname.Trim() == string.Empty) return false;
                if (EditPlants.Employee.Name.Trim() == string.Empty) return false;
                if (EditPlants.Employee.Patronymic.Trim() == string.Empty) return false;
                if (EditPlants.Employee.WorkExpirience == string.Empty) return false;
                if (EditPlants.Sort.Trim() == string.Empty) return false;
                if (EditPlants.Age.Trim() == string.Empty) return false;

                    return true;
            }
            catch { return false; }
        }

    }
}
