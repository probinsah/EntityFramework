namespace DbOperationsWithEFCoreApp.Data
{
    public class BookPrice
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public float Amount { get; set; }
        public int CurrencyId { get; set; }

        public Book Book { get; set; }
        public CurrencyType CurrencyType { get; set; }
    }
}
