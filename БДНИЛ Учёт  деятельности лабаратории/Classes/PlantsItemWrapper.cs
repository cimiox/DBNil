using System.Windows.Input;

namespace БДНИЛ_Учёт__деятельности_лабаратории.Classes
{
    public class PlantsItemWrapper
    {
        //Описание сущности Plants
        public Plants Plants { get; set; }

        //Команды удаление и изменение для сущности Plants
        public ICommand RemoveCmdPlants { get; set; }
        public ICommand EditCmdPlants { get; set; }
    }
}
