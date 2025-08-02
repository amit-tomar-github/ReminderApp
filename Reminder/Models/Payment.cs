using SQLite;

namespace Reminder.Models
{
    public class Payment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
    }
}