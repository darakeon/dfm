/* Acceptance */
alter table acceptance
	modify column ID bigint auto_increment not null;

/* Account */
alter table year
	drop foreign key FK_Year_Account;
alter table schedule
	drop foreign key FK_Schedule_In;
alter table schedule
	drop foreign key FK_Schedule_Out;

alter table account
	modify column ID bigint auto_increment not null;

alter table year
	modify column Account_ID bigint not null,
	add constraint FK_Year_Account
		foreign key (Account_ID)
		references account(ID);
alter table schedule
	modify column In_ID bigint null,
	add constraint FK_Schedule_In
		foreign key (In_ID)
		references account(ID);
alter table schedule
	modify column Out_ID bigint null,
	add constraint FK_Schedule_Out
		foreign key (Out_ID)
		references account(ID);

/* Category */
alter table move
	drop foreign key FK_Move_Category;
alter table schedule
	drop foreign key FK_Schedule_Category;
alter table summary
	drop foreign key FK_Summary_Category;

alter table category
	modify column ID bigint auto_increment not null;

alter table move
	modify column Category_ID bigint null,
	add constraint FK_Move_Category
		foreign key (Category_ID)
		references category(ID);
alter table schedule
	modify column Category_ID bigint null,
	add constraint FK_Schedule_Category
		foreign key (Category_ID)
		references category(ID);
alter table summary
	modify column Category_ID bigint null,
	add constraint FK_Summary_Category
		foreign key (Category_ID)
		references category(ID);

/* Config */
alter table user
	drop foreign key FK_User_Config;

alter table config
	modify column ID bigint auto_increment not null;

alter table user
	modify column Config_ID bigint not null,
	add constraint FK_User_Config
		foreign key (Config_ID)
		references config(ID);

/* Contract */
alter table acceptance
	drop foreign key FK_Acceptance_Contract;

alter table contract
	modify column ID bigint auto_increment not null;

alter table acceptance
	modify column Contract_ID bigint not null,
	add constraint FK_Acceptance_Contract
		foreign key (Contract_ID)
		references contract(ID);

/* Detail */
alter table detail
	modify column ID bigint auto_increment not null;

/* Month */
alter table move
	drop foreign key FK_Move_In;
alter table move
	drop foreign key FK_Move_Out;
alter table summary
	drop foreign key FK_Summary_Month;

alter table month
	modify column ID bigint auto_increment not null;

alter table move
	modify column In_ID bigint null,
	add constraint FK_Move_In
		foreign key (In_ID)
		references month(ID);
alter table move
	modify column Out_ID bigint null,
	add constraint FK_Move_Out
		foreign key (Out_ID)
		references month(ID);
alter table summary
	modify column Month_ID bigint null,
	add constraint FK_Summary_Month
		foreign key (Month_ID)
		references month(ID);

/* Move */
alter table detail
	drop foreign key FK_Detail_Move;

alter table move
	modify column ID bigint auto_increment not null;

alter table detail
	modify column Move_ID bigint null,
	add constraint FK_Detail_Move
		foreign key (Move_ID)
		references move(ID);

/* Schedule */
alter table detail
	drop foreign key FK_Detail_Schedule;
alter table move
	drop foreign key FK_Move_Schedule;

alter table schedule
	modify column ID bigint auto_increment not null;

alter table detail
	modify column Schedule_ID bigint null,
	add constraint FK_Detail_Schedule
		foreign key (Schedule_ID)
		references schedule(ID);
alter table move
	modify column Schedule_ID bigint null,
	add constraint FK_Move_Schedule
		foreign key (Schedule_ID)
		references schedule(ID);

/* Security */
alter table security
	modify column ID bigint auto_increment not null;

/* Summary */
alter table summary
	modify column ID bigint auto_increment not null;

/* Ticket */
alter table ticket
	modify column ID bigint auto_increment not null;

/* User */
alter table acceptance
	drop foreign key FK_Acceptance_User;
alter table account
	drop foreign key FK_Account_User;
alter table category
	drop foreign key FK_Category_User;
alter table schedule
	drop foreign key FK_Schedule_User;
alter table security
	drop foreign key FK_Security_User;
alter table ticket
	drop foreign key FK_Ticket_User;

alter table user
	modify column ID bigint auto_increment not null;

alter table acceptance
	modify column User_ID bigint not null,
	add constraint FK_Acceptance_User
		foreign key (User_ID)
		references user(ID);
alter table account
	modify column User_ID bigint not null,
	add constraint FK_Account_User
		foreign key (User_ID)
		references user(ID);
alter table category
	modify column User_ID bigint not null,
	add constraint FK_Category_User
		foreign key (User_ID)
		references user(ID);
alter table schedule
	modify column User_ID bigint not null,
	add constraint FK_Schedule_User
		foreign key (User_ID)
		references user(ID);
alter table security
	modify column User_ID bigint not null,
	add constraint FK_Security_User
		foreign key (User_ID)
		references user(ID);
alter table ticket
	modify column User_ID bigint not null,
	add constraint FK_Ticket_User
		foreign key (User_ID)
		references user(ID);

/* Year */
alter table month
	drop foreign key FK_Month_Year;
alter table summary
	drop foreign key FK_Summary_Year;

alter table year
	modify column ID bigint auto_increment not null;

alter table month
	modify column Year_ID bigint not null,
	add constraint FK_Month_Year
		foreign key (Year_ID)
		references year(ID);
alter table summary
	modify column Year_ID bigint null,
	add constraint FK_Summary_Year
		foreign key (Year_ID)
		references year(ID);
