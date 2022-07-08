--Creating CgDocs database here
Create Database CgDocs

use CgDocs
go

--Creating users table
create table users(
users_id int primary key not null identity(1,1),
username nvarchar(100),
user_password nvarchar(100),
created_at smalldatetime
);

--Creating folders table
create table folders(
folder_id int primary key not null identity(1,1),
folder_name nvarchar(100),
folder_created_by int,
FOREIGN KEY(folder_created_by) REFERENCES users(users_id),
folder_created_at smalldatetime,
folder_isDeleted bit default 0,
folder_isFavourite bit default 0
);

--Creating documents table
create table documents (
document_id int primary key not null identity(1,1),
document_name nvarchar(20),
document_content_type varchar(30),
document_size int,
document_created_by int,
FOREIGN KEY (document_created_by) REFERENCES users(users_id),
document_created_at smalldatetime,
folderId int,
document_isDeleted bit not null default 0,
FOREIGN KEY (folderId) REFERENCES folders(folder_id),
document_isFavourite bit default 0
);

