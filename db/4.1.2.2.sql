update ticket
	set Expiration = now(6),
		Active = 0,
		Key_ = concat(Key_, date_format(now(6), "%Y%m%d%H%i%S%f"))
	where id <> 0
		and Active = 1
		and Type = 1;
