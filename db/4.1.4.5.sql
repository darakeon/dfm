set sql_safe_updates=0;

update config set timezone='UTC-03:00' where timezone='E. South America Standard Time';
update config set timezone='UTC-03:00' where timezone='Tocantins Standard Time';

set sql_safe_updates=1;

select distinct timezone from config where length(timezone) > 9;

alter table config
	modify timezone varchar(9) not null;
