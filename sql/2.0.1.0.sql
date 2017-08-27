use dfm;

/* before publish */
/*
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

alter table Move
	add ValueCents int null;

alter table Schedule
	add ValueCents int null;
*/

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
	set m.ValueCents = d.ValueCents;

delete from detail
	where move_id in (
		select id
			from move
            where ValueCents is not null
	);
    
    

/* after publish */
/*
alter table detail
	drop Value;

alter table Summary
	drop In_;

alter table Summary
	drop Out_;
*/