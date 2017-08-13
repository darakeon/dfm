ALTER TABLE User
	ADD COLUMN Active tinyint(1) not null default 0;

CREATE TABLE Security (
	ID int(11) NOT NULL AUTO_INCREMENT,
	Active tinyint(1) NOT NULL,
	Expire datetime NOT NULL,
	Action int(11) NOT NULL,
	Sent tinyint(1) NOT NULL,
	User_ID int(11) NOT NULL,
	Token varchar(50) NOT NULL,
	PRIMARY KEY (ID),
	UNIQUE KEY Token (Token),
	KEY User_ID (User_ID),
	CONSTRAINT FKAAE052DE8EFD2E71 FOREIGN KEY (User_ID) REFERENCES user (ID)
);