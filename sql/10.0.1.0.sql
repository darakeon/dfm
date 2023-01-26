insert into migrations (name) values ('10.0.1.0');

create view deleted_users_wipe as
	select id, email, when_ as `when`, why, s3, password, tfa from wipe;
