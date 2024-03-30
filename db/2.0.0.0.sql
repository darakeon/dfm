/*PRE-PUBLISH*/
CREATE TABLE config
(
	ID INT NOT NULL AUTO_INCREMENT,
	Language VARCHAR(5) DEFAULT 'pt-BR' NOT NULL,
	TimeZone VARCHAR(200) NOT NULL DEFAULT 'E. South America Standard Time',
	SendMoveEmail bit NOT NULL DEFAULT 0,
	UseCategories bit NOT NULL DEFAULT 1,
	User_ID INTEGER NOT NULL,
	PRIMARY KEY (ID),
	UNIQUE (User_ID)
);

INSERT INTO config (Language, TimeZone, SendMoveEmail, User_ID)
	SELECT Language, 'E. South America Standard Time', SendMoveEmail, ID
		FROM user;

ALTER TABLE user
	ADD Config_ID INTEGER NULL,
	ADD UNIQUE (Config_ID);

UPDATE user
	INNER JOIN config
	SET user.Config_ID = config.ID
	WHERE user.ID = config.User_ID
		AND user.Config_ID IS NULL;

ALTER TABLE user
	MODIFY COLUMN Config_ID INTEGER NOT NULL;

ALTER TABLE user
	ADD INDEX (Config_ID),
	ADD CONSTRAINT FK_User_Config
	FOREIGN KEY (Config_ID)
	REFERENCES config (ID);

ALTER TABLE config
	DROP COLUMN User_ID,
	DROP INDEX User_ID;

ALTER TABLE ticket
	ADD COLUMN Type INTEGER NOT NULL DEFAULT 0;

/*POS-PUBLISH*/
ALTER TABLE user
	DROP Language,
	DROP SendMoveEmail;

ALTER TABLE move
	MODIFY COLUMN Category_ID INTEGER NULL;

ALTER TABLE summary
	MODIFY COLUMN Category_ID INTEGER NULL;

ALTER TABLE schedule
	MODIFY COLUMN Category_ID INTEGER NULL;

/*FIX FKS*/
SELECT CONCAT('ALTER TABLE ', table_schema, '.', table_name, '\n	DROP FOREIGN KEY ', Constraint_Name, ';') as query
	FROM information_schema.key_column_usage
	WHERE REFERENCED_TABLE_SCHEMA IS NOT NULL
		AND Constraint_Schema = 'dfm';

SELECT /*table_name, column_name,REPLACE(column_name, '_ID', '') as parent_entity,*/
	CONCAT(
		'ALTER TABLE ', table_name, '\n',
		'	ADD CONSTRAINT FK_', UPPER(SUBSTRING(table_name, 1, 1)), LOWER(SUBSTRING(table_name, 2, 100)), '_', REPLACE(column_name, '_ID', ''), '\n',
		'		FOREIGN KEY (', column_name, ')\n',
		'		REFERENCES ',
			CASE REPLACE(column_name, '_ID', '')
				WHEN 'In' THEN
					(CASE table_name
						WHEN 'schedule' THEN 'account'
						WHEN 'move' THEN 'month'
					END)
				WHEN 'Out' THEN
					(CASE table_name
						WHEN 'schedule' THEN 'account'
						WHEN 'move' THEN 'month'
					END)
				ELSE REPLACE(column_name, '_ID', '')
			END,
			' (ID);'
	) as query
	FROM information_schema.columns
	WHERE Table_Schema = 'dfm'
		AND column_name LIKE '%_ID';
