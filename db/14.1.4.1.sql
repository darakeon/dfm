insert into migrations (name) values ('14.1.4.1');


alter table wipe
	add CSVAddress varchar(500) null;

update wipe
	set CSVAddress = S3
	where ID <> 0;

alter table wipe
	drop S3;
