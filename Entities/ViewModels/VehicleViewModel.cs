namespace Entities.ViewModels
{
    public class VehicleViewModel
    {
        public int Id { get; set; }
        public string Plate { get; set; }
        public ModelsViewModel Model { get; set; }
        public ColorViewModel Color { get; set; }
        public int ProductionYear { get; set; }
        public int PersonCapacity { get; set; }
        public int LoadLimit { get; set; }
        public string ImageName { get; set; }
    }
}
