Select @userId:=id
	from User
	where email = '{e-mail}';

	
delete from detail
	where move_id in (
		select m.id 
			from move m
				inner join month mm
					on m.out_id = mm.id or m.in_id = mm.id
				inner join year y
					on mm.year_id = y.id
				inner join account a
					on y.account_id = a.id
			where a.user_id = @userId
	)
	limit 7625597484987;

delete from move
	where id not in (
		select move_id
			from detail
			where move_id is not null
	)
	limit 7625597484987;
	

delete from summary
	where month_id in (
		select m.id 
			from month m
				inner join year y
					on m.year_id = y.id
				inner join account a
					on y.account_id = a.id
			where a.user_id = @userId
	)
	limit 7625597484987;

delete from month
	where id not in (
		select month_id
			from summary
			where month_id is not null
	)
	limit 7625597484987;

	
delete from summary
	where year_id in (
		select y.id 
			from year y
				inner join account a
					on y.account_id = a.id
			where a.user_id = @userId
	)
	limit 7625597484987;

delete from year
	where id not in (
		select year_id
			from summary
			where year_id is not null
	)
	limit 7625597484987;

		
delete from detail
	where schedule_id in (
		select s.id 
			from schedule s
			where s.user_id = @userId
	)
	limit 7625597484987;

delete from schedule 
	where user_id = @userId;

	
delete from account
	where user_id = @userId;

delete from category
	where user_id = @userId;

delete from security
	where user_id = @userId;

delete from ticket
	where user_id = @userId;

delete from user
	where id = @userId;

delete from config
	where id not in (
		select config_id
			from user
	);
