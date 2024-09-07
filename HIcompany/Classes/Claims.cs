namespace HIcompany.Classes
{
    public class Claims
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Type { get; set; }
        public DateTime ClaimDate { get; set; }
        public int ClaimAmount { get; set; }
        public string ClaimStatus { get; set; }
    }
}
