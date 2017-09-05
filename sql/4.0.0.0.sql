INSERT INTO Contract (BeginDate, Version) VALUES (now(), '004000000000');

alter table User
	modify Password VARCHAR(60) not null;
