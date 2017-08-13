Use dfm;

	Alter table Account
		add column Url VARCHAR(20) null;

	Update Account
		set Url = Replace(Replace(Name, " ", "_"), "-", "_")
    where ID > 0;

	Alter table Account
		change Url Url VARCHAR(20) not null;

	Alter table Year
		change Account_ID Account_ID Int Null; 

	Alter table Month
		change Year_ID Year_ID Int Null; 
