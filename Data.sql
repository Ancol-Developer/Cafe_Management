CREATE Database QuanLyQuanCafe
Go
Use QuanLyQuanCafe
Go

-- Food
-- Table
-- FoodCategory
-- Account
-- Bill
-- BillInfor

Create table TableFood
(
	id Int Identity Primary Key,
	name Nvarchar(100) Not null default N'Chưa đặt tên',
	status Nvarchar(100) Not null Default N'Trong' -- Trong|| Co nguoi
)
GO
Create table Account
(
	Username Nvarchar(100) primary key,
	DisplayName nvarchar(100) Not Null default N'Ancol',
	Password Nvarchar(1000) Not Null,
	Type int Not Null default 0 -- 1 : admin || 0 : staff
)
Go
Create Table FoodCetagory
(
	id int Identity primary key,
	name Nvarchar(100) Not null default N'Chưa đặt tên'
)
Go
Create table Food 
(
	id int Identity primary key,
	name nvarchar(100) Not null default N'Chưa đặt tên',
	idCategory Int Not null,
	price float not null Default 0,
	Foreign key (idCategory) references dbo.FoodCetagory(id)
)
Go

Create table Bill
(
	id int Identity primary key,
	DateCheckIn Date Not Null default GetDate(),
	DateCheckOut Date,
	idTable int Not Null,
	status int Not Null default 0 -- 1: da thanh toan || 0: chua thanh toan,
	Foreign key(idTable) references dbo.TableFood(id)
)
Go
Create table BillInfo
(
	id int identity primary key,
	idBill int not null,
	idFood int not null,
	count int not null default 0,
	Foreign key (idBill) references dbo.Bill(id),
	Foreign key (idFood) references dbo.Food(id),
)

insert into dbo.Account(
	Username,DisplayName,Password,Type
)
values(
	N'K9',N'RongK9',N'1',1
)
insert into dbo.Account(
	Username,DisplayName,Password,Type
)
values(
	N'staff',N'staff',N'1',0
)
go

Create proc USP_GetAccountByUserName
@userName nvarchar(100)
As
Begin
	select * from dbo.Account where Username= @userName
End
Go

Exec dbo.USP_GetAccountByUserName @userName = N'K9'
go

Create proc USP_Login
@userName nvarchar(100), @passWord nvarchar(100)
As 
Begin
	Select * from dbo.Account Where Username = @userName and Password=@passWord
End
Go

Exec dbo.USP_Login @userName=N'k9' , @passWord = N'1'
