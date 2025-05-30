alter table config
	add Wizard bit not null;

alter table user
	add TFASecret varchar(32) null;

alter table ticket
	add ValidTFA bit not null;
