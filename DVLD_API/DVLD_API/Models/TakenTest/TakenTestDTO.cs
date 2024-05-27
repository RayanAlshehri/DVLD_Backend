namespace DVLD_API.Models.TakenTest
{
    public class TakenTestDTO
    {
        public int AppointmentID { get; set; }
        public bool Result { get; set; }
        public string Notes { get; set; }
        public int? CreatedByUserID { get; set; }
    }
}
