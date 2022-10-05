using Water_BIlls;

var usersNames = new[]
{
    "منظمة أجيال بلا قات",
    "مكتب إدارة المشاريع الخيرية",
    "الجمعية الخيرية",
    "مكتب إدارة الجمعية الخيرية",
    "البنك اليمني للإنشاء والتعمير",
    "سكن هاني الحزمي"
};
var usersNameInEnglish = new[]
{
    "GWQ",
    "Charity Project Management",
    "Charity",
    "Charity Office",
    "YBRD",
    "Hani Alhazmi's apartment"
    
};

new PDFGenerator().Print(GetUsersInput());

BillCounter.User[] GetUsersInput()
{
    var users = new BillCounter.User[usersNames.Length];

    var unitCost = SetUnitCost();
    
    for (var i = 0; i < users.Length; i++)
    {
        users[i].Name = GetName(i);
        users[i].LastUnit = SetLastUnit();
        users[i].CurrentUnit = SetCurrentUnit();
        users[i].UnitCost = unitCost;
        users[i].Arrears = SetArrears();
    }
    return users;
    
    string GetName(int index)
    {
        Print(usersNameInEnglish[index]);
        return usersNames[index];
    }
    
    double SetUnitCost()
    {
        Print("Set unit cost for this month please");
        return GetIntInput();
    }

    int SetLastUnit()
    {
        Print("Set last unit please");
        return GetIntInput();
    }
    
    int SetCurrentUnit()
    {
        Print("Set current unit please");
        return GetIntInput();
    }
    
    int SetArrears()
    {
        Print("Set arrears if any");
        return GetIntInput(true);
    }
    
    int GetIntInput(bool skippable = false)
    {
        goBack:
        var line = Console.ReadLine();
        if(int.TryParse(line, out var value)) return value;//to prevent nigative values
        if (skippable) return 0;
        goto goBack; 
    }
    void Print(string log) => Console.WriteLine(log);
} 