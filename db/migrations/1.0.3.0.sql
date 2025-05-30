Alter Table detail
	add Schedule_ID int null,
	add index (Schedule_ID),
	add constraint FK_Detail_Schedule
		foreign key (Schedule_ID)
		references schedule (ID);

Alter Table schedule
	add Description varchar(50) null,
	add Nature int null,
	add LastRun smallint not null default 0,
	add Deleted smallint not null default 0,
	add In_ID int null,
	add index (In_ID),
	add constraint FK_Schedule_In
		foreign key (In_ID)
		references account (ID),
	add Out_ID int null,
	add index (Out_ID),
	add constraint FK_Schedule_Out
		foreign key (Out_ID)
		references account (ID),
	add Category_ID int null,
	add index (Category_ID),
	add constraint FK_Schedule_Category
		foreign key (Category_ID)
		references category (ID);

Update detail D
	inner join (
		Select s.ID SID, Min(f.id) FID
			from schedule s
				inner join FutureMove f
					on s.ID = f.Schedule_ID
			group by s.ID
	) sf
		on D.FutureMove_ID = sf.FID
	set D.Schedule_ID = sf.SID,
		D.FutureMove_ID = null
	where D.ID <> 0;

Update schedule s
	inner join (
		Select s.ID SID, Min(f.id) FID
			from schedule s
				inner join FutureMove f
					on s.ID = f.Schedule_ID
			group by s.ID
	) sf
		on s.ID = sf.SID
	inner join FutureMove f
		on sf.FID = f.ID
	set s.Description = f.Description,
		s.Nature = f.Nature,
		s.In_ID = f.In_ID,
		s.Out_ID = f.Out_ID,
		s.Category_ID = f.Category_ID
	where s.ID <> 0;

Update schedule s
	inner join (
		Select Schedule_ID, Count(*) Qte
			from move
			group by Schedule_ID
	) m
		on s.ID = m.Schedule_ID
	set s.LastRun = m.Qte
	where id <> 0;

Alter Table detail
	DROP FOREIGN KEY FK321CBF8F90E5CDC4;

Alter Table detail
	DROP COLUMN FutureMove_ID,
	DROP INDEX FutureMove_ID;

Drop Table FutureMove;

Alter Table schedule
	modify Description varchar(50) not null,
	modify Category_ID int not null,
	modify Nature int not null,
	change Begin Date datetime not null;

Alter table summary
	add Broken bit not null default 0;

CREATE TABLE ticket (
	ID integer NOT NULL AUTO_INCREMENT,
	Key_ varchar(52) NOT NULL,
	Active bit NOT NULL DEFAULT 1,
	Expiration datetime DEFAULT NULL,
	Creation datetime NOT NULL,
	User_ID integer NOT NULL,
	PRIMARY KEY (ID),
	UNIQUE KEY Key_ (Key_),
	KEY User_ID (User_ID),
	CONSTRAINT FK_Ticket_User
		FOREIGN KEY (User_ID)
		REFERENCES user (ID)
);
