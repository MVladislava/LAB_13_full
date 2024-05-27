using Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_13
{
    public class MyObservableCollection<T> : MyCollection<T> where T : IInit, IComparable, new()
    {
        public delegate void CollectionHandler(object source, CollectionHandlerEventArgs args); // Делегат для события изменения коллекции
        
        public event CollectionHandler CollectionCountChanged;// Событие при изменении количества элементов в коллекции

        public event CollectionHandler CollectionReferenceChanged;// Событие при изменении ссылок на элементы в коллекции

        public MyObservableCollection(int length) : base(length) { }
        // Метод добавления элемента в коллекцию
        public new void Add(T obj)
        {
            base.Add(obj);
            CollectionCountChanged?.Invoke(this, new CollectionHandlerEventArgs($"Элемент {obj} добавлен в коллекцию.", obj));
        }

        // Метод удаления элемента из коллекции
        public new bool Remove(T obj)
        {
            bool result = base.Remove(obj);
            if (result)
            {
                CollectionCountChanged?.Invoke(this, new CollectionHandlerEventArgs($"Элемент {obj} удален из коллекции.", obj));
            }
            return result;
        }

        // Переопределение set индексатора для события CollectionReferenceChanged
        public new T this[T val]
        {
            set
            {
                base[val] = value;
                CollectionReferenceChanged?.Invoke(this, new CollectionHandlerEventArgs($"Элемент {val} заменен на {value}.", value));
            }
        }
        // Метод для вызова события CollectionCountChanged
        //protected virtual void OnCollectionCountChanged(string message, object affectedObject)
        //{
        //    CollectionCountChanged?.Invoke(this, new CollectionHandlerEventArgs(message, affectedObject));
        //}

        //// Метод для вызова события CollectionReferenceChanged
        //protected virtual void OnCollectionReferenceChanged(string message, object affectedObject)
        //{
        //    CollectionReferenceChanged?.Invoke(this, new CollectionHandlerEventArgs(message, affectedObject));
        //}
        //public void FillCollection(int count, Func<T> itemGenerator) // Метод для заполнения коллекции элементами, сгенерированными функцией itemGenerator
        //{
        //    base.Clear(); // Очищаем коллекцию перед заполнением
        //    for (int i = 0; i < count; i++)
        //    {
        //        Add(itemGenerator());
        //    }
        //    OnCollectionChanged();
        //}
    }

}
