create schema dfm;

create user 'dfm_user'@'%'; 
grant insert, update, delete, select
	on dfm.* to 'dfm_user'@'%';


create user 'dfm_creator'@'%' IDENTIFIED BY 'password';

grant insert, update, delete, select,
	create, alter, drop,
	references, index, trigger,
	create view,
	create temporary tables
	on dfm.* to 'dfm_creator'@'%';

update mysql.user
	set Super_Priv='Y'
	where user='dfm_creator'
		and host='%';
