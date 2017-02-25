using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace БДНИЛ_Учёт__деятельности_лабаратории.Classes
{
    class Query
    {
        public void QueryAddTableEmployee(DataGrid dg) {
            using (Entities db = new Entities()) {
                try {  
                    var query = (from employee in db.Employee
                             join plants in db.Plants on employee.IdEmployee equals plants.EmployeeId
                             select new
                             {
                                 ID = employee.IdEmployee,
                                 Фамилия = employee.Surname,
                                 Имя = employee.Name,
                                 Отчество = employee.Patronymic,
                                 Опыт = employee.WorkExpirience,
                                 Выработка = employee.DayPerfomance,
                                 Сорт = plants.Sort,
                                 Количество = plants.Count,
                                 Возраст = plants.Age
                             }).ToList();

                    dg.ItemsSource = query;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ошибка вывода данных");
                }
            }
        }

        public void QueryAddTablePlants(DataGrid dg)
        {
            using (Entities db = new Entities())
            {
                try
                {
                    var query = (from plants in db.Plants
                                 join client in db.Client on plants.EmployeeId equals client.IdClient
                                 join bankClient in db.BankClient on client.AccountClientId equals bankClient.IdAccountClient
                                 join adress in db.Adress on client.AdressId2 equals adress.IdAdress
                                 select new
                                 {
                                     Id = client.IdClient,
                                     Тип = client.TypeOfInstitutionalUnits,
                                     Объем = client.OrderVolume,
                                     Заявка = client.TheProspectOfFurtherCooperation,
                                     Счёт = bankClient.AccountOfTheClient,
                                     Информация = adress.ContactPhone + " " + adress.Adress1,
                                     Сорт = plants.Sort,
                                     Возраст = plants.Age
                                 }).ToList();
                    dg.ItemsSource = query;
                }
                catch(Exception e) { MessageBox.Show("Ошибка вывода данных"); }
            }
        }

        public void QueryAddTableProvider(DataGrid dg)
        {
            using (Entities db = new Entities())
            {
                try
                {
                    var query = (from provider in db.Provider
                                 join adress in db.Adress on provider.AdressId1 equals adress.IdAdress
                                 join resource in db.Resource on provider.ResourceId equals resource.IdResource
                                 join bankProvider in db.BankProvider on provider.AccountSupplierId equals bankProvider.IdAccountSupplier
                                 select new
                                 {
                                     ID = provider.IdProvider,
                                     Наименование = provider.NameOfSupplier,
                                     Информация = adress.Adress1 + " " + adress.ContactPhone,
                                     Счёт = bankProvider.AccountOfTheSupplier,
                                     Тип = resource.TypeOfRawMaterial,
                                     Количество = resource.CountResource,
                                     Цена = resource.Cost

                                 }).ToList();
                    dg.ItemsSource = query;
                }
                catch (Exception e) { MessageBox.Show("Ошибка вывода данных"); }
            }
        }

        public void QueryAddTableUser(DataGrid dg)
        {
            using (Entities db = new Entities())
            {
                try
                {
                    var query = (from users in db.Users
                                 join userRoles in db.UserRoles on users.UserRoleId equals userRoles.IdUserRole
                                 select new
                                 {
                                     Логин = users.Login,
                                     Имя = users.FName,
                                     Фамилия = users.LName,
                                     Отчество = users.UserPatronymic,
                                     Роль = userRoles.Title,
                                 }).ToList();
                    dg.ItemsSource = query;
                }
                catch (Exception e) { MessageBox.Show("Ошибка вывода данных"); }
            }
        }
    }
}
