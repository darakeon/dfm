alter table Config
	add Wizard bit not null;

alter table User
	add TFASecret varchar(32) null;
