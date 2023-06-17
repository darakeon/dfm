insert into migrations (name) values ('9.0.0.0');
insert into migrations (name) values ('9.0.1.0');
insert into migrations (name) values ('10.0.1.0');

drop trigger no_update_purge;
drop trigger no_delete_purge;


alter table wipe
	add HashedEmail varchar(72),
	add UsernameStart varchar(2),
	add DomainStart varchar(3),
	add Theme smallint,
	add Language varchar(5),
	modify Email varchar(320) null,
	drop TFA;

set sql_safe_updates = 0;

update wipe
	set UsernameStart = left(Email, 2),
		DomainStart = left(right(Email, length(Email) - position('@' in Email)), 3),
		Theme = -2,
		Language = 'pt-BR';

set sql_safe_updates = 1;

alter table wipe
	modify UsernameStart varchar(2) not null,
	modify DomainStart varchar(3) not null,
	modify Theme smallint not null,
	modify Language varchar(5) not null;

create view deleted_users_wipe as
	select id,
		HashedEmail as hashed_email,
		UsernameStart as username_start,
		DomainStart as domain_start,
		when_ as `when`, why,
		password, s3,
		email
	from wipe;


create trigger no_update_wipe
	before update
	on `wipe` for each row
		signal sqlstate '45000'
			set message_text = 'cannot update wipe';
create trigger no_delete_wipe
	before delete
	on `wipe` for each row
		signal sqlstate '45000'
			set message_text = 'cannot delete wipe';

alter table security
	add wipe_id bigint null,
	add constraint FK_Security_Wipe
		foreign key (wipe_id)
		references wipe (id),
	modify user_id bigint null;


delimiter //

create procedure finish_wipe_table()
begin
	if exists(select * from wipe where HashedEmail is null) then
		select "Run python script to fill HashedEmail";
	else
		alter table wipe
			modify HashedEmail varchar(60) not null,
			drop Email;
	end if;
end //

call finish_wipe_table;

drop procedure finish_wipe_table;
