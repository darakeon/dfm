alter table ticket
	add lastAccess datetime;

set sql_safe_updates = 0;
update ticket set lastAccess = now();
set sql_safe_updates = 1;

alter table ticket
	modify lastAccess datetime not null;


alter table account
	add open bit;

set sql_safe_updates = 0;
update account set open = 1 where endDate is null;
update account set open = 0 where endDate is not null;
set sql_safe_updates = 1;

alter table account
	modify open bit not null;
