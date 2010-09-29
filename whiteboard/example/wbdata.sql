drop table if exists wbdata;

create table wbdata (
	data_id integer not null primary key auto_increment,
	board_id integer,
	data blob);