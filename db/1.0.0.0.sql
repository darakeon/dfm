create table account (
	ID INTEGER NOT NULL AUTO_INCREMENT,
	Name VARCHAR(20) not null,
	EndDate DATETIME,
	RedLimit DOUBLE,
	YellowLimit DOUBLE,
	BeginDate DATETIME not null,
	User_ID INTEGER not null,
	primary key (ID)
);

create table category (
	ID INTEGER NOT NULL AUTO_INCREMENT,
	Name VARCHAR(20) not null,
	Active BIT default 1 not null,
	User_ID INTEGER not null,
	primary key (ID)
);

create table detail (
	ID INTEGER NOT NULL AUTO_INCREMENT,
	Description VARCHAR(50) not null,
	Amount SMALLINT default 1 not null,
	Value DOUBLE not null,
	Move_ID INTEGER not null,
	primary key (ID)
);

create table month (
	ID INTEGER NOT NULL AUTO_INCREMENT,
	Time SMALLINT not null,
	Year_ID INTEGER not null,
	primary key (ID)
);

create table move (
	ID INTEGER NOT NULL AUTO_INCREMENT,
	Description VARCHAR(50) not null,
	Date DATETIME not null,
	Nature INTEGER not null,
	In_ID INTEGER,
	Out_ID INTEGER,
	Schedule_ID INTEGER,
	Category_ID INTEGER not null,
	primary key (ID)
);

create table schedule (
	ID INTEGER NOT NULL AUTO_INCREMENT,
	Active BIT default 1 not null,
	Times SMALLINT not null,
	Boundless BIT not null,
	Begin DATETIME not null,
	Next DATETIME not null,
	Frequency INTEGER not null,
	User_ID INTEGER not null,
	primary key (ID)
);

create table summary (
	ID INTEGER NOT NULL AUTO_INCREMENT,
	In_ DOUBLE not null,
	Out_ DOUBLE not null,
	Nature INTEGER not null,
	Month_ID INTEGER,
	Year_ID INTEGER,
	Category_ID INTEGER not null,
	primary key (ID),
	unique (Month_ID, Year_ID, Category_ID)
);

create table user (
	ID INTEGER NOT NULL AUTO_INCREMENT,
	Password VARCHAR(50) not null,
	Email VARCHAR(50) not null unique,
	Language VARCHAR(5) default 'pt-BR' not null,
	primary key (ID)
);

create table year (
	ID INTEGER NOT NULL AUTO_INCREMENT,
	Time SMALLINT not null,
	Account_ID INTEGER not null,
	primary key (ID)
);

alter table account
	add index (User_ID),
	add constraint FK_Account_User
	foreign key (User_ID)
	references user (ID);

alter table category
	add index (User_ID),
	add constraint FK_Category_User
	foreign key (User_ID)
	references user (ID);

alter table detail
	add index (Move_ID),
	add constraint FK_Detail_Move
	foreign key (Move_ID)
	references move (ID);

alter table month
	add index (Year_ID),
	add constraint FK_Month_Year
	foreign key (Year_ID)
	references year (ID);

alter table move
	add index (In_ID),
	add constraint FK_Move_InMonth
	foreign key (In_ID)
	references month (ID);

alter table move
	add index (Out_ID),
	add constraint FK_Move_OutMonth
	foreign key (Out_ID)
	references month (ID);

alter table move
	add index (Schedule_ID),
	add constraint FK_Move_Schedule
	foreign key (Schedule_ID)
	references schedule (ID);

alter table move
	add index (Category_ID),
	add constraint FK_Move_Category
	foreign key (Category_ID)
	references category (ID);

alter table schedule
	add index (User_ID),
	add constraint FK_Schedule_User
	foreign key (User_ID)
	references user (ID);

alter table summary
	add index (Month_ID),
	add constraint FK_Summary_Month
	foreign key (Month_ID)
	references month (ID);

alter table summary
	add index (Year_ID),
	add constraint FK_Summary_Year
	foreign key (Year_ID)
	references year (ID);

alter table summary
	add index (Category_ID),
	add constraint FK_Summary_Category
	foreign key (Category_ID)
	references category (ID);

alter table year
	add index (Account_ID),
	add constraint FK_Year_Account
	foreign key (Account_ID)
	references account (ID);
