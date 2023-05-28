using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForm.Model
{
    public class ResultDataModel : ICloneable
    {
        public string Mit_number { get; set; } //Регистрационный номер типа
        public string Mit_title { get; set; } //Наименование типа
        public string Mit_notation { get; set; } //Обозначение типа
        public string Mi_modification { get; set; } //Модицикация
        public string Mi_number { get; set; } //Заводской серийный номер
        public string signCipher { get; set; } //Шифр клейма
        public string regNumber { get; set; } //Регистрационный номер СИ в Перечне
        public string rankCode { get; set; } //Код разряда эталона в ГПС, которому соответствует СИ
        public string schemaTitle { get; set; } //Наименование разряда эталона в ГПС, которому соответствует СИ
        public string Verification_date { get; set; } //Дата поверки
        public string Org_title { get; set; } //Наименование организации-поверителя
        public string Inn { get; set; } //ИНН

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
