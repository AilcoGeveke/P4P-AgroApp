namespace AWA.Models
{
    public class Cargo
    {
        public int CargoId { set; get; }
        public long Date { set; get; }
        public string Type { set; get; }
        public int FullLoad { set; get; }
        public int EmptyLoad { set; get; }
        public int NetLoad { set; get; }
        public string Direction { set; get; }
    }
}
