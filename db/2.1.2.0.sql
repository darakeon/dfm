alter table move
	modify nature smallint not null;

alter table schedule
	modify nature smallint not null,
	modify frequency smallint not null;

alter table security
	modify action smallint not null;

alter table summary
	modify nature smallint not null;

alter table ticket
	modify type smallint not null;
