using System;
using System.Collections.Generic;
using FileExchange.Importer;

namespace FileExchange.Domain
{
    //Nature will be decided according with Accounts filling
    public class Record
    {
        public Record(IImporter importer, Int32 row)
        {
            AccountIn = importer["AccountIn", row];
            AccountOut = importer["AccountOut", row];
            Category = importer["Category", row];
            Description = importer["Description", row];
            Position = row;


            DateTime date;
            var dateIsValid = DateTime.TryParse(importer["Date", row], out date);
            Date = date;

            Double value;
            var valueIsValid = Double.TryParse(importer["Value", row], out value);
            Value = value;


            var microRecordListIsValid = makeMicroRecords(importer, row);


            Status = isValid(dateIsValid, valueIsValid, microRecordListIsValid)
                    ? RecordStatus.Fail
                    : RecordStatus.OnSystem;

        }


        private Boolean makeMicroRecords(IImporter importer, Int32 row)
        {
            MicroRecord microRecord;
            var position = 0;
            var isValid = true;

            do
            {
                position++;

                microRecord = new MicroRecord(importer, row, position);

                if (!microRecord.IsEmpty)
                    break;

                microRecord.RecordID = ID;
                MicroRecordList.Add(microRecord);

                isValid &= microRecord.IsValid;
            } while (true);

            return isValid;
        }


        private Boolean isValid(Boolean dateIsValid, Boolean valueIsValid, Boolean microRecordListIsValid)
        {
            return !dateIsValid || !valueIsValid || microRecordListIsValid ||
                   String.IsNullOrEmpty(AccountIn) || String.IsNullOrEmpty(AccountOut) ||
                   String.IsNullOrEmpty(Description);
        }



        public Int32 ID { get; set; }
        public Int32 FileID { get; set; }
        
        public String AccountIn { get; set; }
        public String AccountOut { get; set; }
        public String Category { get; set; }
        public String Description { get; set; }
        public DateTime Date { get; set; }
        public Double Value { get; set; }

        public IList<MicroRecord> MicroRecordList { get; set; }

        public RecordStatus Status { get; set; }
        public Int32 Position { get; set; }
    }
    
}
