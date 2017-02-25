using System.Windows.Input;

namespace БДНИЛ_Учёт__деятельности_лабаратории.Classes
{
    public class UsersItemWrapper
    {
        //Описание сущности Users
        public Users Users { get; set; }

        //Команды удаление и изменение для сущности Users
        public ICommand RemoveCmd { get; set; }
        public ICommand EditCmd { get; set; }
    }
}
