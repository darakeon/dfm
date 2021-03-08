set sql_safe_updates = 0;

update config
	set theme = 1
	where theme > 0;

update config
	set theme = -1
	where theme < 0;

set sql_safe_updates = 1;
