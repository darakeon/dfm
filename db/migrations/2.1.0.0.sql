alter table category
	modify Active bit not null;

alter table config
	modify SendMoveEmail bit not null,
	modify UseCategories bit not null;

alter table schedule
	modify Active bit not null,
	modify ShowInstallment bit not null,
	modify Boundless bit not null;

alter table security
	modify Active bit not null,
	modify Sent bit not null;

alter table summary
	modify Broken bit not null;

alter table ticket
	modify Active bit not null;

alter table user
	modify Active bit not null;

alter table move
	drop checked;

alter table move
	add Checked bit not null;

alter table config
	add MoveCheck bit not null;
