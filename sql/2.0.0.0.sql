ALTER TABLE PageLog CHANGE COLUMN IP IP VARCHAR(50) NOT NULL;

ALTER TABLE User
	Add TimeZone varchar(200) not null default 'E. South America Standard Time'

ALTER TABLE Ticket
	Add MachineId varchar(100) null