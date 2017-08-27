use dfm;

/* before publish */
alter table Detail
	add ValueCents int null;

update detail
	set ValueCents = Value * 100;

alter table detail
	modify ValueCents int not null;

alter table Summary
	add InCents int null;

update Summary
	set InCents = In_ * 100;

alter table Summary
	modify InCents int not null;

alter table Summary
	add OutCents int null;

update Summary
	set OutCents = Out_ * 100;

alter table Summary
	modify OutCents int not null;



/* after publish */
alter table detail
	drop Value;

alter table Summary
	drop In_;

alter table Summary
	drop Out_;

