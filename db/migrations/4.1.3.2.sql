alter table detail
	drop foreign key FK_Detail_Move;

alter table detail
	modify column Move_ID integer null;

alter table detail
	add constraint FK_Detail_Move
		foreign key (Move_ID)
		references move (ID);
