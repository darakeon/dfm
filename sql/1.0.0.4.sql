use dfm;

alter table User
	ADD COLUMN SendMoveEmail tinyint(1) not null default 0;
