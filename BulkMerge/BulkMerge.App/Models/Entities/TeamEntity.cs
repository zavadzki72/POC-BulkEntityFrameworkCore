namespace BulkMerge.App.Models.Entities
{
    public class TeamEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Initials { get; set; }
        public string Country { get; set; }
        public DateTime Founded_At { get; set; }
        public int StadiumId { get; set; }
        
        public virtual StadiumEntity Stadium { get; set; }
    }
}
