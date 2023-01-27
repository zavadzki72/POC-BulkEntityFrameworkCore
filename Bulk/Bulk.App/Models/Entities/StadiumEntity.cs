namespace Bulk.App.Models.Entities
{
    public class StadiumEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }
        public int Capacity { get; set; }

        public virtual TeamEntity Team { get; set; }
    }
}
