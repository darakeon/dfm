/* MONTH */

select * from (
	select s.id S, c.name C, m.time M, y.time Y, a.Name A, s.OutCents SO
			, ifnull(sum(mo.valuecents), 0) + ifnull(sum(do.valuecents * do.amount), 0) MO
		from summary s
			inner join category c
				on s.Category_ID = c.id
			inner join month m
				on s.month_id = m.id
			inner join year y
				on m.year_id = y.id
			inner join account a
				on y.account_id = a.id
			left join move mo
				on mo.Out_ID = m.id
					and s.Category_ID = mo.Category_ID
				left join detail do
					on mo.id = do.move_id
		where a.user_ID = 18
		group by s.id
    ) as summaries
	where (SO <> MO);
    
select * from (
	select s.id S, c.name C, m.time M, y.time Y, a.Name A, s.InCents SI
			, ifnull(sum(mi.valuecents), 0) + ifnull(sum(di.valuecents * di.amount), 0) MI
		from summary s
			inner join category c
				on s.Category_ID = c.id
			inner join month m
				on s.month_id = m.id
			inner join year y
				on m.year_id = y.id
			inner join account a
				on y.account_id = a.id
			left join move mi
				on mi.In_ID = m.id
					and s.Category_ID = mi.Category_ID
				left join detail di
					on mi.id = di.move_id
		where a.user_ID = 18
		group by s.id
    ) as summaries
	where (SI <> MI);
	

	
/* YEAR */

select * from (
	select s.id S, c.name C, m.time M, y.time Y, a.Name A, s.OutCents SO
			, ifnull(sum(mo.valuecents), 0) + ifnull(sum(do.valuecents * do.amount), 0) MO
		from summary s
			inner join category c
				on s.Category_ID = c.id
			inner join year y
				on s.year_id = y.id
			inner join account a
				on y.account_id = a.id
			inner join month m
				on m.year_id = y.id
			left join move mo
				on mo.Out_ID = m.id
					and s.Category_ID = mo.Category_ID
				left join detail do
					on mo.id = do.move_id
		where a.user_ID = 18
		group by s.id
    ) as summaries
	where (SO <> MO);
    
select * from (
	select s.id S, c.name C, m.time M, y.time Y, a.Name A, s.InCents SI
			, ifnull(sum(mi.valuecents), 0) + ifnull(sum(di.valuecents * di.amount), 0) MI
		from summary s
			inner join category c
				on s.Category_ID = c.id
			inner join year y
				on s.year_id = y.id
			inner join account a
				on y.account_id = a.id
			inner join month m
				on m.year_id = y.id
			left join move mi
				on mi.In_ID = m.id
					and s.Category_ID = mi.Category_ID
				left join detail di
					on mi.id = di.move_id
		where a.user_ID = 18
		group by s.id
    ) as summaries
	where (SI <> MI);