alter table detail
	add column FutureMove_ID INTEGER;

alter table schedule
	add column ShowInstallment bit;

create table FutureMove (
	ID INTEGER NOT NULL AUTO_INCREMENT,
   Description VARCHAR(50) not null,
   Date DATETIME not null,
   Nature INTEGER not null,
   Schedule_ID INTEGER,
   Category_ID INTEGER not null,
   In_ID INTEGER,
   Out_ID INTEGER,
   primary key (ID)
);

alter table detail
	add index (FutureMove_ID),
	add constraint FK321CBF8F90E5CDC4
	foreign key (FutureMove_ID)
	references FutureMove (ID);

alter table FutureMove
	add index (Schedule_ID),
	add constraint FKFAF045B96C85B666
	foreign key (Schedule_ID)
	references schedule (ID);

alter table FutureMove
	add index (Category_ID),
	add constraint FKFAF045B944FCFB39
	foreign key (Category_ID)
	references category (ID);

alter table FutureMove
	add index (In_ID),
	add constraint FKFAF045B9707068F
	foreign key (In_ID)
	references account (ID);

alter table FutureMove
	add index (Out_ID),
	add constraint FKFAF045B961325821
	foreign key (Out_ID)
	references account (ID);

alter table schedule
	drop column next;
