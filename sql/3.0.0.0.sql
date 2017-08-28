CREATE TABLE Contract
(
	ID INT NOT NULL,
	BeginDate DATETIME NOT NULL,
	Version VARCHAR(50) NOT NULL,
	
	CONSTRAINT PK_Contract
		PRIMARY KEY (ID),
		
	CONSTRAINT UK_Contract
		UNIQUE KEY (Version)
);

CREATE TABLE Acceptance
(
	ID INT NOT NULL,
	Accept BIT NOT NULL,
	CreateDate DATETIME NOT NULL,
	AcceptDate DATETIME NOT NULL,
	Contract_ID INT NOT NULL,
	User_ID INT NOT NULL,
	
	CONSTRAINT PK_Acceptance
		PRIMARY KEY (ID),
		
	CONSTRAINT FK_Acceptance_Contract
		FOREIGN KEY (Contract_ID)
		REFERENCES Contract (ID),
		
	CONSTRAINT FK_Acceptance_User
		FOREIGN KEY (User_ID)
		REFERENCES User (ID),
		
	CONSTRAINT UK_Acceptance
		UNIQUE KEY (User_ID, Contract_ID)
);

INSERT INTO Contract (BeginDate, Version) VALUES (curdate(), '3.0.0.0');

INSERT INTO Acceptance
	(CreateDate, Contract_ID, User_ID)
	SELECT getdate(), 
		FROM Contract c
			JOIN User u
		WHERE c.Version = '3.0.0.0';
