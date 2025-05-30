set sql_safe_updates = 0;
update move m
	inner join (
		select move_id, sum(valueCents * amount) valueCents
			from detail
		    where move_id is not null
		    group by move_id
	) d
		on m.id = d.move_id
	set m.valueCents = d.valueCents
	where m.valueCents is null;
set sql_safe_updates = 1;

alter table move
	modify valueCents int not null;

set sql_safe_updates = 0;
update schedule s
	inner join (
		select schedule_id, sum(valueCents * amount) valueCents
			from detail
		    where schedule_id is not null
		    group by schedule_id
	) d
		on s.id = d.schedule_id
	set s.valueCents = d.valueCents
	where s.valueCents is null;
update schedule
	set valueCents = 0, active = 0
	where valueCents is null;
set sql_safe_updates = 1;

alter table schedule
	modify valueCents int not null;
