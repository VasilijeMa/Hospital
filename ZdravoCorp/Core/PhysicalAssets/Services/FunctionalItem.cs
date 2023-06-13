namespace ZdravoCorp.Core.PhysicalAssets.Services
{
    public class FunctionalItem
    {
        public string Where { get; set; }
        public string What { get; set; }
        public int Amount { get; set; }
        public FunctionalItem(string where, string what, int amount)
        {
            Where = where;
            What = what;
            Amount = amount;
        }

        public void SetAmount(int amount)
        {
            Amount = amount;
        }

        public string GetWhere() { return Where; }
        public string GetWhat() { return What; }
        public int GetAmount() { return Amount; }
        public string ToString()
        {
            return What + " " + Where + " " + Amount.ToString();
        }
    }
}

