insert into migrations (name) values ('14.3.0.2');

drop view deleted_users_wipe;

create view deleted_users_wipe as
	select id,
		HashedEmail as hashed_email,
		UsernameStart as username_start,
		DomainStart as domain_start,
		when_ as `when`,
		why,
		password,
		CsvAddress as csv_address
	from wipe;
