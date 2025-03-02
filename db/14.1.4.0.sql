insert into migrations (name) values ('14.1.4.0');

ALTER TABLE control
    ADD WrongTFA INT NOT NULL DEFAULT 0;
