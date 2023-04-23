namespace MyFirstApp.Entities
{
    public class CalendarEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateEnd { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
    }
}
