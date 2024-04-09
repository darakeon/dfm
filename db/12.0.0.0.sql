insert into migrations (name) values ('12.0.0.0');

alter table settings
	add column UseCurrency bit not null default(0);

alter table account
	add column Currency smallint null;

drop table month;
drop table year;
