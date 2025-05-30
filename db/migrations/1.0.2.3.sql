Alter table account
	add column Url VARCHAR(20) null;

Update account
	set Url = Replace(Replace(Name, " ", "_"), "-", "_")
where ID > 0;

Alter table account
	change Url Url VARCHAR(20) not null;

Alter table year
	change Account_ID Account_ID Int Null;

Alter table month
	change Year_ID Year_ID Int Null;
