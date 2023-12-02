create schema dfm;

create user 'dfm_user'@'%'; 
grant insert, update, delete, select on dfm.* to 'dfm_user'@'%';

use dfm;
