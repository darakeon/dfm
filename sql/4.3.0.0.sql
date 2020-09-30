alter table ticket
	add lastAccess datetime;

set sql_safe_updates = 0;
update ticket set lastAccess = now();
set sql_safe_updates = 1;

alter table ticket
	modify lastAccess datetime not null;
