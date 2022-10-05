namespace Water_BIlls;

public class BillCounter
{
    public struct User
    {
        public string Name;
        public int LastUnit;
        public int CurrentUnit;
        public int UnitDifference => CurrentUnit - LastUnit;
        public double UnitCost;
        public double Cost => UnitDifference * UnitCost;
        public double Arrears;
        public double Total => Cost + Arrears; 

        public User(string name, int lastUnit, int currentUnit, double arrears, double unitPrice, double unitCost)
        {
            Name = name;
            LastUnit = lastUnit;
            CurrentUnit = currentUnit;
            UnitCost = unitCost;
            Arrears = arrears;
        }
    }
}