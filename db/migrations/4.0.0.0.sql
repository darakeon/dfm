INSERT INTO contract (BeginDate, Version) VALUES (now(), '004000000000');

alter table user
	modify Password VARCHAR(60) not null;
