use dfm;

ALTER TABLE PageLog CHANGE COLUMN IP IP VARCHAR(50) NOT NULL;

/*PRE-PUBLISH*/
CREATE TABLE Config
(
	ID INT NOT NULL AUTO_INCREMENT,
	Language VARCHAR(5) DEFAULT 'pt-BR' NOT NULL,
	TimeZone VARCHAR(200) NOT NULL DEFAULT 'E. South America Standard Time',
	SendMoveEmail TINYINT(1) NOT NULL DEFAULT 0,
	UseCategories TINYINT(1) NOT NULL DEFAULT 1,
	User_ID INTEGER NOT NULL,
	PRIMARY KEY (ID),
	UNIQUE (User_ID)
);

INSERT INTO Config (Language, TimeZone, SendMoveEmail, User_ID)
	SELECT Language, 'E. South America Standard Time', SendMoveEmail, ID
		FROM User;
	
ALTER TABLE User
	ADD Config_ID INTEGER NULL,
	ADD UNIQUE (Config_ID);

UPDATE User
	INNER JOIN Config
	SET User.Config_ID = Config.ID
	WHERE User.ID = Config.User_ID
		AND User.Config_ID IS NULL;

ALTER TABLE User
	MODIFY COLUMN Config_ID INTEGER NOT NULL;

ALTER TABLE User
	ADD INDEX (Config_ID), 
	ADD CONSTRAINT FK_User_Config
	FOREIGN KEY (Config_ID)
	REFERENCES Config (ID);

ALTER TABLE Config
	DROP COLUMN User_ID,
	DROP INDEX User_ID;
	
ALTER TABLE Ticket
	ADD COLUMN Type INTEGER NOT NULL DEFAULT 0;
	
	
	
/*POS-PUBLISH*/
ALTER TABLE User
	DROP Language,
	DROP SendMoveEmail;
	
ALTER TABLE Move
	MODIFY COLUMN Category_ID INTEGER NULL;

ALTER TABLE Summary
	MODIFY COLUMN Category_ID INTEGER NULL;

ALTER TABLE Schedule
	MODIFY COLUMN Category_ID INTEGER NULL;


/*FIX FKS*/
SELECT CONCAT('ALTER TABLE ', table_schema, '.', table_name, '\nDROP FOREIGN KEY ', Constraint_Name, ';') as query
    FROM information_schema.key_column_usage
    WHERE REFERENCED_TABLE_SCHEMA IS NOT NULL
        AND Constraint_Schema = 'dfm';

SELECT table_name, column_name,REPLACE(column_name, '_ID', '') as parent_entity,
    CONCAT(
        'ALTER TABLE ', table_name, '\n',
        '   ADD CONSTRAINT FK_', UPPER(SUBSTRING(table_name, 1, 1)), LOWER(SUBSTRING(table_name, 2, 100)), '_', REPLACE(column_name, '_ID', ''), '\n',
        '       FOREIGN KEY (', column_name, ')\n',
        '       REFERENCES ',  
            CASE REPLACE(column_name, '_ID', '')
                WHEN 'In' THEN 
                    (CASE table_name
                        WHEN 'Schedule' THEN 'Account'
                        WHEN 'Move' THEN 'Month'
                    END)
                WHEN 'Out' THEN 
                    (CASE table_name
                        WHEN 'Schedule' THEN 'Account'
                        WHEN 'Move' THEN 'Month'
                    END)
                ELSE REPLACE(column_name, '_ID', '')
            END,
            ' (ID);'
    ) as query
    FROM information_schema.columns
    WHERE Table_Schema = 'dfm'
        AND column_name LIKE '%_ID';
