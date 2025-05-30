create temporary table initial_moves
	select in_id, out_id,
		cast(
			concat(year, "-", month, "-", day)
		    as Date
		) as date
	from move;

create temporary table accounts_to_fix
	select a.id, min(m.date) new, a.BeginDate old
		from account a
			inner join initial_moves m
				on m.in_id = a.id
					or m.out_id = a.id
	group by a.id
		having min(m.date) < a.BeginDate;

set sql_safe_updates = 0;

update account a
	inner join accounts_to_fix f
		on a.id = f.id
			and a.begindate = f.old
	set a.begindate = f.new;

set sql_safe_updates = 1;

drop temporary table accounts_to_fix;
drop temporary table initial_moves;

alter table account
	add constraint name_user
		unique key (Name, User_ID),
	add constraint url_user
		unique key (Url, User_ID);

alter table category
	add constraint name_user
		unique key (Name, User_ID);
