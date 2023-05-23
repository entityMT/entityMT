create database Examples

use Examples
go

create table Tutor
(
	tutorId int identity(1,1) primary key,
	tutorName varchar(255) not null,
	document varchar(255) not null
)

create table Pet
(
	petId int identity(1,1) primary key,
	petName varchar(255) not null,
	years int not null,
	document varchar(50),
	tutorId int not null,

	constraint fk_pet_tutor_tutorId foreign key (tutorId) references Tutor(tutorId)
)

create table PetMedicalHistory
(
	petMedicalHistoryId int identity(1,1) primary key,
	petId int not null,
	appointmentDate datetime not null,
	comments varchar(max),
	
	constraint fk_petMedicalHistory_pet_petId foreign key(petId) references Pet(petId)
)


--inserts:

insert into Tutor values('Eric', '50-834-455-X')
insert into Tutor values('Eva', '50-834-455-X')

insert into Pet values('Hana', 5, null, 1)
insert into Pet values('Harry', 4, null, 1)
insert into Pet values('John',3,'500-800',2)

insert into PetMedicalHistory values(3, '2023-02-05', 'V8 - vaccine first dose')
insert into PetMedicalHistory values(3, '2023-03-05', 'V8 - vaccine final dose')
insert into PetMedicalHistory values(3, '2023-04-05', 'V10 - vaccine unique dose')