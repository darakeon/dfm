update schedule
	set Active = 0
	where Active = 1
		and (out_ID in (select a.id from account a where a.EndDate is not null)
		or in_ID in (select a.id from account a where a.EndDate is not null))
		and id <> 0;

/* before publish */
alter table account
	add YellowLimitCents int null,
	add RedLimitCents int null;

update account
	set YellowLimitCents = YellowLimit * 100,
		RedLimitCents = RedLimit * 100
	where id <> 0;

/* after publish */
alter table account
	drop YellowLimit,
	drop RedLimit;
