insert into migrations (name) values ('9.0.0.0');
insert into migrations (name) values ('9.0.1.0');
insert into migrations (name) values ('10.0.1.0');

create view deleted_users_wipe as
	select id, email, when_ as `when`, why, s3, password, tfa from wipe;


drop trigger no_update_purge;
create trigger no_update_wipe
	before update
	on `wipe` for each row
		signal sqlstate '45000'
			set message_text = 'cannot update wipe';

drop trigger no_delete_purge;
create trigger no_delete_wipe
	before delete
	on `wipe` for each row
		signal sqlstate '45000'
			set message_text = 'cannot delete wipe';
