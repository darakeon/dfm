alter table user
	add IsRobot bit not null default 0,
    add RobotCheck datetime null;

set sql_safe_updates = 0;
update user
	set RobotCheck = NOW();
set sql_safe_updates = 1;
    
alter table user
    modify RobotCheck datetime not null;
