alter table move		add ExternalId longblob not null;
alter table detail		add ExternalId longblob not null;
alter table schedule	add ExternalId longblob not null;

set sql_safe_updates = 0;
update move		set ExternalId = uuid_to_bin(uuid());
update detail	set ExternalId = uuid_to_bin(uuid());
update schedule	set ExternalId = uuid_to_bin(uuid());
set sql_safe_updates = 1;

alter table move		add unique(ExternalId(16));
alter table detail		add unique(ExternalId(16));
alter table schedule	add unique(ExternalId(16));
