/* before publish */
alter table detail
	add ValueCents int null;

update detail
	set ValueCents = Value * 100
	where id <> 0;

alter table detail
	modify ValueCents int not null;

alter table summary
	add InCents int null;

update summary
	set InCents = In_ * 100
	where id <> 0;

alter table summary
	modify InCents int not null;

alter table summary
	add OutCents int null;

update summary
	set OutCents = Out_ * 100
	where id <> 0;

alter table summary
	modify OutCents int not null;

alter table move
	add ValueCents int null;

alter table schedule
	add ValueCents int null;

update move m
	inner join detail d
		on m.ID = d.move_id
	inner join
		(select d.Move_ID
			from detail d
				inner join move m
					on d.Move_ID = m.ID
			where d.Description = m.Description
				and d.Amount = 1
			group by d.Move_ID
			having count(*) = 1) g
		on m.ID = g.move_id
	set m.ValueCents = d.ValueCents
	where m.id <> 0;

set sql_safe_updates = 0;
delete from detail
	where move_id in (
		select id
			from move
			where ValueCents is not null
	) and id <> 0;
set sql_safe_updates = 1;

/* after publish */
alter table detail
	drop Value;

alter table summary
	drop In_;

alter table summary
	drop Out_;
