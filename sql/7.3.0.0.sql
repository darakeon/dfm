set sql_safe_updates = 0;

update Ticket
	set type = type + 1;

set sql_safe_updates = 1;
