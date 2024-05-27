namespace LAB_13
{
    public class CollectionHandlerEventArgs : EventArgs
    {
        // Открытое автореализуемое свойство типа string с информацией о типе изменений в коллекции
        public string ChangeType { get; private set; }

        // Открытое автореализуемое свойство для ссылки на объект, с которым связаны изменения
        public object AffectedObject { get; private set; }

        // Конструкторы для инициализации класса
        public CollectionHandlerEventArgs(string changeType, object affectedObject)
        {
            ChangeType = changeType;
            AffectedObject = affectedObject;
        }
    }
}