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
	status Nvarchar(100) Not null Default N'Trống' -- Trong|| Co nguoi
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

-- Thêm bàn
DECLARE @i int =0
While @i<=10
Begin
	Insert dbo.TableFood(name) Values (N'Bàn '+ CAST(@i As nvarchar(100)))	
	Set @i=@i+1
End

Update dbo.TableFood
Set status = N'Có người'
where id=3

Select * from dbo.TableFood
go

Create proc USP_GetTableList
As 
Begin
	Select * From dbo.TableFood
End
go

EXEC dbo.USP_GetTableList
 
-- thêm category
Insert dbo.FoodCetagory
(name)
Values (N'Hải sản')
Insert dbo.FoodCetagory
(name)
Values (N'Nông sản')
Insert dbo.FoodCetagory
(name)
Values (N'Lâm sản')
Insert dbo.FoodCetagory
(name)
Values (N'Nước')
-- thêm món ăn
Insert dbo.Food
(name,idCategory,price)
Values (N'Mực một nắng nướng sa tế',1,120000)
Insert dbo.Food
(name,idCategory,price)
Values (N'Nghêu hấp xả',1,50000)
Insert dbo.Food
(name,idCategory,price)
Values (N'Dê nướng',2,170000)
Insert dbo.Food
(name,idCategory,price)
Values (N'Heo rừng nướng',3,160000)
Insert dbo.Food
(name,idCategory,price)
Values (N'Cafe',4,20000)

--thêm bill
Insert dbo.Bill
(DateCheckin,DateCheckOut,idTable,status)
values(GETDATE(),Null,1,0)
Insert dbo.Bill
(DateCheckin,DateCheckOut,idTable,status)
values(GETDATE(),Null,2,0)
Insert dbo.Bill
(DateCheckin,DateCheckOut,idTable,status)
values(GETDATE(),GetDate(),3,1)
--thêm bill infor
Insert dbo.BillInfo
(idBill,idFood,count)
values(1,1,2)
Insert dbo.BillInfo
(idBill,idFood,count)
values(1,3,4)
Insert dbo.BillInfo
(idBill,idFood,count)
values(1,5,1)
Insert dbo.BillInfo
(idBill,idFood,count)
values(2,1,2)
Insert dbo.BillInfo
(idBill,idFood,count)
values(2,5,2)
Insert dbo.BillInfo
(idBill,idFood,count)
values(3,1,2)
go


Select * from dbo.Bill
Select * From dbo.BillInfo
Select * From dbo.TableFood
Select * From Dbo.Food
Select * from dbo.FoodCetagory

Select f.name,bi.count,f.price,f.price*bi.count as totalPrice from dbo.BillInfo as bi, dbo.Bill as b,dbo.Food as f
Where bi.idBill = b.id and bi.idFood=f.id and b.idTable=3

go
Create Proc USP_InsertBill
@idTable int
As
Begin
	Insert dbo.Bill (DateCheckIn,DateCheckOut,idTable,status,discount)
	Values (GetDate(),Null,@idTable,0,0)
End
Go

Create Proc USP_InsertBillInfo
@idBill int, @idFood int,@count int
As
Begin
	Declare @isExitsBillInfo int
	Declare @foodCount int = 1
	Select @isExitsBillInfo = id, @foodCount = b.count from dbo.BillInfo as b where idBill =@idBill And idFood=@idFood

	If(@isExitsBillInfo>0)
	Begin 
		Declare @newCount Int = @foodCount + @count
		if(@newCount>0)
			Update dbo.BillInfo Set count= @foodCount + @count where idFood=@idFood
		Else
			Delete dbo.BillInfo Where idBill= @idBill and idFood= @idFood
	End
	Else
	Begin
		Insert dbo.BillInfo (idBill,idFood,count)
		Values (@idBill,@idFood,@count)
	End
End
Go

Update dbo.Bill Set status =1 where id=1

Select Max(id) from dbo.Bill

-- Cap nhat thong tin co nguoi
go
Alter Trigger UTG_UpdateBillInfo
On dbo.BillInfo For Insert, Update
As
Begin
	Declare @idBill int
	Select @idBill = idBill From inserted
	Declare  @idTable int
	Select @idTable = idTable from dbo.Bill Where id = @idBill and status = 0
	Declare @countBillInfo int
	Select @countBillInfo = Count(*) from dbo.BillInfo Where idBill = @idBill
	Declare @count int 
	Select @count = Count(*) From dbo.BillInfo Where idBill = @idBill
	If(@count>0)
		Update dbo.TableFood set status = N'Có người' where id = @idTable
	Else
		Update dbo.TableFood set status = N'Trong' where id = @idTable
End
Go
-- Cap nhat thong tin trong
Create trigger UTG_UpdateBill
On dbo.Bill For Update
As
Begin	
	Declare @idBill int
	Select @idBill = id from inserted
	Declare  @idTable int
	Select @idTable = idTable from dbo.Bill Where id = @idBill
	Declare @count int = 0
	Select @count = COUNT(*) From dbo.Bill Where idTable =@idTable and status = 0
	If(@count = 0)
		Update dbo.TableFood Set status = N'Trong' where id = @idTable
End
Go
--
--
Delete dbo.BillInfo
Delete dbo.Bill

Alter Table dbo.Bill
Add discount int

Update dbo.Bill Set discount = 0


Go
Alter Proc USP_SwitchTable
@idTable1 int, @idTable2 int
As Begin
	Declare @idFristBill int
	Declare @idSecondBill int
	Declare @isFristTableEmpty int = 1
	Declare @isSecondTableEmpty int = 1

	Select @idSecondBill = id from dbo.Bill Where idTable = @idTable2 and status = 0
	Select @idFristBill = id from dbo.Bill Where idTable = @idTable1 and status = 0

	If(@idFristBill is null)
	Begin
		Insert dbo.Bill (DateCheckIn,DateCheckOut,idTable,status) values (GetDate(),null,@idTable1,0)
		Select @idFristBill = Max(id) From dbo.Bill where idTable = @idTable1 and status =0 
	End

	Select @isFristTableEmpty = COUNT(*) From dbo.BillInfo Where idBill = @idFristBill

	If(@idSecondBill is null)
	Begin
		Insert dbo.Bill (DateCheckIn,DateCheckOut,idTable,status) values (GetDate(),null,@idTable2,0)
		Select @idSecondBill = Max(id) From dbo.Bill where idTable = @idTable2 and status =0 
	End

	Select @isSecondTableEmpty = COUNT(*) From dbo.BillInfo Where idBill = @idSecondBill
	

	Select id Into IDBillInfoTable From dbo.BillInfo Where idBill = @idSecondBill
	UpDate dbo.BillInfo Set idBill = @idSecondBill Where idBill = @idFristBill 
	Update dbo.BillInfo Set idBill = @idFristBill where id in (Select * from IDBillInfoTable)
	DROP TABLE IDBillInfoTable
	if(@isFristTableEmpty = 0)
		Update dbo.TableFood Set status = N'Trong' Where id = @idTable2
	if(@isSecondTableEmpty = 0)
		Update dbo.TableFood Set status = N'Trong' Where id = @idTable1
End
Go

exec USP_SwitchTable @idTable1 = 2 ,@idTable2 = 3 
