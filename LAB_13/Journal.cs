using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_13
{
    public class JournalEntry
    {
        public string CollectionName { get; private set; }

        // Тип изменений в коллекции
        public string ChangeType { get; private set; }

        // Данные объекта, с которым связаны изменения в коллекции
        public string AffectedObjectData { get; private set; }

        // Конструктор для инициализации полей класса
        public JournalEntry(string collectionName, string changeType, string affectedObjectData)
        {
            CollectionName = collectionName;
            ChangeType = changeType;
            AffectedObjectData = affectedObjectData;
        }

        // Перегруженная версия метода ToString()
        public override string ToString()
        {
            return $"Изменения в коллекции '{CollectionName}': {ChangeType}. ";
        }
    }

    // Класс Journal для хранения информации об изменениях в коллекции
    public class Journal
    {
        public List<JournalEntry> Entries { get; private set; } = new List<JournalEntry>();

        public void AddEntry(object source, CollectionHandlerEventArgs args)
        {
            string collectionName = source.GetType().Name;
            string changeType = args.ChangeType;
            string affectedObjectData = args.AffectedObject.ToString();

            Entries.Add(new JournalEntry(collectionName, changeType, affectedObjectData));
        }

        // Перегруженная версия метода ToString() для вывода всего журнала
        public override string ToString()
        {
            string result = "Журнал изменений:\n";
            foreach (var entry in Entries)
            {
                result += entry.ToString() + "\n";
            }
            return result;
        }
    }
}
