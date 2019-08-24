alter table move
	add checked bit not null;

alter table category
	modify active bit not null default 1;

alter table config
	modify sendmoveemail bit not null default 0,
	modify usecategories bit not null default 1;

alter table schedule
	modify active bit not null default 1,
	modify showinstallment bit not null,
	modify boundless bit not null;

alter table security
	modify active bit not null,
	modify sent bit not null;

alter table summary
	modify broken bit not null;

alter table ticket
	modify active bit not null;

alter table user
	modify active bit not null;
