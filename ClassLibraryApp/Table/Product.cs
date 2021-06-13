namespace ClassLibraryApp.Table
{
    public class Product
    {
        public int ID { get; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }

        public Product(int id, string name, string model, string description, Category category)
        {
            ID = id;
            Name = name;
            Model = model;
            Description = description;
            Category = category;
        }

        public override string ToString()
        {
            Description = Description ?? " ";
            return $"ID - {ID}: \n\tНазвание - {Name}, Модель - {Model} \n\tОписание - {Description} \n\tКатегория - {Category.Name}";
        }
    }
}
