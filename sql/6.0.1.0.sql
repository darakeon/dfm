alter table user
	add IsRobot bit not null default 0,
    add RobotCheck datetime null;

set sql_safe_updates = 0;
update user
	set RobotCheck = NOW();
set sql_safe_updates = 1;
    
alter table user
    modify RobotCheck datetime not null;

create trigger no_update_contract
	before update
	on contract for each row
		signal sqlstate '45000'
			set message_text = 'cannot update contract';

create trigger no_delete_contract
	before delete
	on contract for each row
		signal sqlstate '45000'
			set message_text = 'cannot delete contract';

create trigger no_update_terms
	before update
	on terms for each row
		signal sqlstate '45000'
			set message_text = 'cannot update terms';

create trigger no_delete_terms
	before delete
	on terms for each row
		signal sqlstate '45000'
			set message_text = 'cannot delete terms';

alter table terms
	add unique key UK_Terms (Contract_ID, Language);
