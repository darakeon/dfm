alter table user
	add IsAdm bit not null default 0;

alter table security
	modify ID integer not null auto_increment;

alter table security
	modify User_ID integer not null;

alter table ticket
	modify ID integer not null auto_increment;

alter table ticket
	modify User_ID integer not null;
