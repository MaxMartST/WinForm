namespace WinForm.Model
{
    public class CompanyData
    {
        public string Inn { get; set; }
        public string Kpp { get; set; }
        public string Ogrn { get; set; }
        public string ShortNameCompany { get; set; }
        public string FullNameCompany { get; set; }
        public string FullAddres { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Mail { get; set; }
        public string GetNamePersone()
        {
            return $"{Surname} {Name} {Patronymic}";
        }
    }
}
