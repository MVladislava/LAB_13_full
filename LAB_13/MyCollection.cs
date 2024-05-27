using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Cars;

namespace LAB_13
{
    public class MyCollection<T> : IEnumerable<T>, ICollection<T> where T : IInit, IComparable, new()
    {
        public Point<T>? root = null;
        int count = 0;
        public int Count => count;

        public bool IsReadOnly => false;

        public MyCollection(int length)
        {
            count = length;
            root = MakeTree(length, root);
        }
        public int Lenght => count;
        public void ShowTree()
        {
            Show(root);
        }
        Point<T>? MakeTree(int length, Point<T>? point) // идеально сбалансированное дерево - за счёт постоянного подсчёта оптимального количства веток слева и справа
        {
            T data = new T();
            data.RandomInit();
            Point<T> newItem = new Point<T>(data);
            if(length == 0)
            {
                return null;
            }
            int nl = length / 2; // определяем количество элементов для левой ветки
            int nr = length - nl - 1; // определяем количество элементов для правой ветки
            newItem.Left = MakeTree(nl, newItem.Left); // создаём ветки
            newItem.Right = MakeTree(nr, newItem.Right);
            return newItem;
        }
        void Show(Point<T>? point, int spaces = 5) // передаём корень поддерева и количество пробелов 
        {
            if(point != null)
            {
                Show(point.Right, spaces + 5);
                for (int i = 0; i < spaces; i++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine(point.Data);
                Show(point.Left, spaces + 5);
            }
        }
        //дерево поиска
         public void AddPoint(T data) 
        {
            Point<T> point = root;
            Point<T> current = null;
            bool isExist = false;
            while(point != null && !isExist)
            {
                current = point;
                if(point.Data.CompareTo(data) == 0) // элемент уже есть
                    isExist = true;
                else // ищем место
                {
                    if(point.Data.CompareTo(data) < 0)
                        point = point.Left;
                    else
                        point = point.Right;
                }
            }
            // нашли место
            if (isExist) // если такой элемент уже есть
                return;
            Point<T> newPoint = new Point<T>(data);
            if(current.Data.CompareTo(data) < 0) // если элемент меньше корня добавляем влево, иначе вправо
                current.Left = newPoint;
            else
                current.Right = newPoint;
            count++;
        }
        public T FindMax()
        {
            Point<T>? current = root;

            if (current == null)
            {
                throw new InvalidOperationException("Дерево пусто");
            }

            while (current.Right != null)
            {
                current = current.Right;
            }

            return current.Data;
        }
        //индексатор с методами get set
        public T this[T val]
    {
        get
        {
            Point<T>? foundPoint = Find(val);
            if (foundPoint == null)
                throw new ArgumentOutOfRangeException();
            return foundPoint.Data;
        }
        set
        {
            if (RemovePoint(val))
            {
                Add(value);
            }
        }
    }
        public T[] ToArray()
        {
            T[] array = new T[count];
            int current = 0;
            TransformToArray(root, array, ref current);
            return array;
        }
        void TransformToArray(Point<T>? point, T[]array, ref int current) // получение массива из дерева (рекурсией)
        {
            if(point != null ) // проверяем, что корень поддерва не пустой
            {
                TransformToArray(point.Left, array, ref current); //идём в левое поддерево
                array[current] = point.Data; //дошли до самого левого узла и кладём его в массив
                current++;
                TransformToArray(point.Right, array, ref current);
            }
        }
        public void TransformToFindTree() // по полученному массиву строим дерево поиска
        {
            T[] array = new T[count];
            int current = 0;
            TransformToArray(root, array, ref current); // сложили все элементы из ИСД 
            root = new Point<T>(array[0]); // создаём корень
            count = 0;
            for (int i = 1; i < array.Length; i++) // идём по массиву и добавляем туда элементы
            {
                AddPoint(array[i]);
            }
        }
        private Point<T>? Find(T data)
        {
            Point<T>? current = root;
            while (current != null)
            {
                int comparison = data.CompareTo(current.Data);
                if (comparison == 0)
                    return current;
                else if (comparison < 0)
                    current = current.Left;
                else
                    current = current.Right;
            }
            return null;
        }
        public bool RemovePoint(T key)
        {
            Point<T> parent = null;
            Point<T> current = root;
            while(current != null && !current.Data.Equals(key))
            {
                parent = current;
                if (current.Data.CompareTo(key) < 0)
                    current = current.Left;
                else
                    current = current.Right;
            }
            if(current ==  null)
                return false;
            if (current.Left == null && current.Right == null) // у узла нет дочерних узлов
            {
                if (parent == null) // удаляем узел
                {
                    root = null;
                }
                else if (parent.Left == current)
                {
                    parent.Left = null;
                }
                else
                {
                    parent.Right = null;
                }
            }
            else if (current.Left == null) // усли у узла есть только правый дочерний узел
            {
                if (parent == null)
                {
                    root = current.Right; // обновляем ссылку родительского узла на правый дочерний узел
                }
                else if (parent.Left == current)
                {
                    parent.Left = current.Right;
                }
                else
                {
                    parent.Right = current.Right;
                }
            }
            else if (current.Right == null) // усли у узла есть только левый дочерний узел
            {
                if (parent == null)
                {
                    root = current.Left; // обновляем ссылку родительского узла на левый дочерний узел
                }
                else if (parent.Left == current)
                {
                    parent.Left = current.Left;
                }
                else
                {
                    parent.Right = current.Left;
                }
            }
            // Если у узла есть два дочерних узла
            else
            {
                Point<T>? sParent = current;
                Point<T>? s = current.Right;

                // Находим минимальный элемент в правом поддереве
                while (s.Left != null)
                {
                    sParent = s;
                    s = s.Left;
                }
                if (sParent != current) // Если нашли преемника в правом поддереве, перемещаем его данные в текущий узел
                {
                    sParent.Left = s.Right;
                    s.Right = current.Right;
                }
                // Обновляем ссылки родителя на текущий узел
                if (parent == null)
                {
                    root = s;
                }
                else if (parent.Left == current)
                {
                    parent.Left = s;
                }
                else
                {
                    parent.Right = s;
                }

                s.Left = current.Left;
            }

            count--;
            return true;
        }
        public void Delete() // удаление дерева
        {
            root = null;
            count = 0;
        }

        public void Add(T data)
        {
            AddPoint(data);
        }

        public void Clear()
        {
            root = null;
            count = 0;
        }

        public bool Contains(T item)
        {
            return Find(item) != null;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var item in this)
            {
                array[arrayIndex++] = item;
            }
        }

        public bool Remove(T item)
        {
            return RemovePoint(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Traverse(root).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<T> Traverse(Point<T>? point)
        {
            if (point != null)
            {
                foreach (var item in Traverse(point.Left))
                {
                    yield return item;
                }
                yield return point.Data;
                foreach (var item in Traverse(point.Right))
                {
                    yield return item;
                }
            }
        }

    }
}
