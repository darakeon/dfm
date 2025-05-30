set sql_safe_updates = 0;

update ticket
	set type = type + 1;

set sql_safe_updates = 1;

create table tips (
	ID bigint auto_increment not null,
	Type smallint not null,
	Temporary bigint unsigned not null, 
	Permanent bigint unsigned not null,
	Last bigint unsigned not null,
	Countdown smallint not null,
	Repeat_ smallint not null,
	User_ID bigint not null,
	primary key (ID),
	constraint FK_tips_user
		foreign key (User_ID)
		references user (ID)
);
