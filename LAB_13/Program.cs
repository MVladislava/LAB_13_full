
using Cars;
using LAB_12_3;
using System.ComponentModel.Design;

namespace LAB_13
{
    internal class Program
    { 
        private static void Main(string[] args)
        {
            // Создание двух коллекций MyObservableCollection<Car>
            var collection1 = new MyObservableCollection<Car>(3);
            var collection2 = new MyObservableCollection<Car>(3);

            // Создание двух объектов Journal
            var journal1 = new Journal();
            var journal2 = new Journal();

            // Подписка первого журнала на события первой коллекции
            collection1.CollectionCountChanged += journal1.AddEntry;
            collection1.CollectionReferenceChanged += journal1.AddEntry;

            // Подписка второго журнала на события из обеих коллекций
            collection1.CollectionReferenceChanged += journal2.AddEntry;
            collection2.CollectionReferenceChanged += journal2.AddEntry;

            var car1 = new Car { Brand = "BMW", Year_Issue = 2021, Color = "Черный", Price = 1000000, GroundClearance = 150 };
            var car2 = new Car { Brand = "Audi", Year_Issue = 2020, Color = "Белый", Price = 1200000, GroundClearance = 140 };
            var car3 = new Car { Brand = "Toyota", Year_Issue = 2019, Color = "Красный", Price = 900000, GroundClearance = 160 };
            Menu choise = new Menu();
            MyObservableCollection<Car> tree = new MyObservableCollection<Car>(10);
            while (true)
            {
                // Вывод данных обоих журналов
                Console.WriteLine("Данные журнала 1:");
                Console.WriteLine(journal1);

                Console.WriteLine("Данные журнала 2:");
                Console.WriteLine(journal2);
                choise.MenuChoise();
                var str = Console.ReadLine();
                int l = int.Parse(str);
                Console.Clear();
                switch (l)
                {
                    case 1:
                        foreach (Car c in tree)
                        {
                            Console.WriteLine(c);
                        }
                        break;
                    case 2: // Внесение изменений в коллекции
                        collection1.Add(car1);
                        collection1.Add(car2);
                        collection2.Add(car3);

                        collection1.Remove(car1);
                        collection2.Remove(car3);
                        break;
                    case 3: // Присвоение новому элементу нового значени
                        var originalCar = car2;
                        var modifiedCar = originalCar.ShallowCopy();
                        modifiedCar.Brand = "Mercedes";
                        collection1[originalCar] = modifiedCar;
                        break;
                    case 4:
                        tree.Delete();
                        Console.WriteLine("Дерево удалено!)");
                        break;
                }
            }
        }
    }
}