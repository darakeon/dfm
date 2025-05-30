alter table move
	add column Position smallint null;

set sql_safe_updates = 0;
update move m
	inner join (
		select m.ID, count(b.ID)+1 position
			from move m
				inner join schedule s
					on m.Schedule_ID = s.ID
				left join move b
					on b.Schedule_ID = s.ID
						and b.ID < m.ID
			group by s.ID, m.ID
			order by s.ID, m.ID
	) p
		on m.id = p.id
	set m.position = p.position
	where m.position is null;
set sql_safe_updates = 1;
