using System;
using FileExchange.Importer;

namespace FileExchange.Domain
{
    public class MicroRecord
    {
        public MicroRecord(IImporter importer, Int32 row, Int32 position)
        {
            var strAmount = importer["Amount" + position, row];
            var strValue = importer["Value" + position, row];
            var description = importer["Description" + position, row];

            if (String.IsNullOrEmpty(strAmount) && String.IsNullOrEmpty(strValue) && String.IsNullOrEmpty(description))
            {
                IsEmpty = true;
                return;
            }


            Int32 amount;
            IsValid = Int32.TryParse(strAmount, out amount);
            Amount = amount;

            Double value;
            IsValid &= Double.TryParse(strValue, out value);
            Value = value;

            Description = description;
            IsValid &= !String.IsNullOrEmpty(Description);
        }
        

        public Int32 ID { get; set; }
        public Int32 RecordID { get; set; }
    
        public String Description { get; set; }
        public Int32 Amount { get; set; }
        public Double Value { get; set; }

        public Boolean IsEmpty { get; set; }
        public Boolean IsValid { get; set; }

    }

}
