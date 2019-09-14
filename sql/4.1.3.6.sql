alter table move
	drop foreign key FK_Move_In,
	drop foreign key FK_Move_Out,
	change column In_ID In_ID_old bigint null,
	change column Out_ID Out_ID_old bigint null;

alter table move
	add column In_ID bigint null,
    add constraint FK_Move_In
		foreign key (In_ID)
        references account(ID),
	add column Out_ID bigint null,
    add constraint FK_Move_Out
		foreign key (Out_ID)
        references account(ID);

update move x
	inner join month m
		on x.in_id_old = m.id
	inner join year y
		on m.year_id = y.id
	set x.in_id = y.account_ID
    where x.id <> 0;

update move x
	inner join month m
		on x.out_id_old = m.id
	inner join year y
		on m.year_id = y.id
	set x.out_id = y.account_ID
    where x.id <> 0;

alter table move
	add column Day smallint not null,
	add column Month smallint not null,
	add column Year smallint not null,
	modify column Date datetime null;

update move
	set Day = day(Date),
		Month = month(Date),
		Year = year(Date)
	where id <> 0;

alter table summary
	drop foreign key FK_Summary_Month,
	drop foreign key FK_Summary_Year;

alter table summary
	add column Account_ID bigint null,
    add constraint FK_Summary_Account
		foreign key (Account_ID)
        references account(ID),
	add column Time int not null;

update summary s
	inner join month m
		on s.month_id = m.id
	inner join year y
		on m.year_id = y.id
	set s.account_id = y.account_ID,
		s.time = y.time * 100 + m.time
	where s.id <> 0;

update summary s
	inner join year y
		on s.year_id = y.id
	set s.account_id = y.account_ID,
		s.time = y.time
	where s.id <> 0;

alter table summary
	modify column Account_ID bigint not null;

alter table summary
	add constraint UK_Summary
		unique key (Account_ID, Nature, Time, Category_ID);
