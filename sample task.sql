create table company
(
companyid int primary key identity(1,1),
companycode varchar(30),
companyname varchar(30)
)
insert into company values('cmp','test company'),('abc','software company'),('sda','hardware company'),('vns','trading company'),('sss','food company')
select * from company

create table companyuser
(
userid int identity(1,1),
useremail varchar(30),
password varchar(30),
companyuserid int foreign key references company(companyid),
isactive bit,
isdelete bit
)
insert into companyuser values('test@gmail.com','dGVzdGluZw==',1,1,0),('software@gmail.com','c29mdHdhcmU=',2,1,0),
('hardware@gmail.com','aGFyZHdhcmU=',3,1,0),('trading@gmail.com','dHJhZGluZw==',4,0,1),('food@gmail.com','Zm9vZA==',5,1,1)
select * from companyuser
drop table companyuser

create table itemmaster
(
itemid int identity(1,1),
companyiditem int foreign key references company(companyid),
itemcode varchar(30),
itemdescription varchar(40),
quantity int,
itemamount decimal,
isactive bit,
isvoid bit,
)

insert into itemmaster values(1,'item1','DESC',2,1000,1,0),(2,'item1','DESC',8,1000,1,0),(3,'item1','DESC',6,1000,1,0),(4,'item1','DESC',8,1000,1,0),(5,'item1','DESC',2,1000,1,0),
                              (4,'item2','DESC',6,500,1,0),(4,'item3','DESC',4,250,1,0)
select * from itemmaster
drop table itemmaster

create table tradingparty_master
(
masterid int identity(1,1),
mastercompanyid int foreign key references company(companyid),
email varchar(30),
isinitiator bit,
isimporter bit,
isexporter bit,
parentid int,
isactive bit,
isvoid bit
)
insert into tradingparty_master values(1,'testin@gmail.com',1,1,1,0,1,0),(2,'testim@gmail.com',1,1,1,0,1,0),(3,'testex@gmail.com',1,1,1,0,1,0),(4,'testini@gmail.com',1,1,1,1,1,0),(5,'testingparty@gmail.com',1,1,1,0,1,0),(4,'testimi@gmail.com',1,1,1,1,1,0),(4,'testexp@gmail.com',1,1,1,1,1,0)
select * from tradingparty_master


drop table tradingparty_master

create table orderheader
(
orderid int primary key identity(1,1),
ordercompanyid int foreign key references company(companyid),
orderno varchar(30),
orderdate varchar(30),
jobtype varchar(30),
initiator int,
importer int,
exporter int,
referencesno varchar(30),
userid int,
isactive bit,
isvoid bit,
)
select * from orderheader
select t.mastercompanyid,o.ordercompanyid from tradingparty_master t join orderheader o on t.mastercompanyid=o.ordercompanyid

drop table orderheader

delete from orderheader
truncate table orderheader

create table orderinvoice
(
invoiceid int identity(1,1),
orderid int foreign key references orderheader(orderid),
invoiceno varchar(30),
invoicedate varchar(30),
invoicetype varchar(30),
currencycode varchar(30),
exchangerate decimal,
invoiceamount decimal,
invoicelocalvalue decimal,
inschargetype varchar(30),
insvalue decimal,
inslocalvalue decimal,
othchargetype varchar(30),
othvalue decimal,
othlocalvalue decimal,
costins decimal,
costoth decimal,
costinsoth decimal,
userid int,
isactive bit,
isvoid bit,
)
select * from orderinvoice

drop table orderinvoice

truncate table orderinvoice

select exchangerate,invoiceamount from orderinvoice

create table itemsections
(
itemid int identity(1,1),
orderid int foreign key references orderheader(orderid),
invoiceno varchar(30),
itemcode varchar(30),
itemdescription varchar(30),
quantity int,
itemamount decimal,
itemlocalvalue decimal,
unitprice decimal,
userid int,
isactive bit,
isvoid bit,
)

select * from itemsections

drop table itemsections

truncate table itemsections


CREATE PROCEDURE GetOrderHeader
    @OrderId INT
AS
BEGIN
    SELECT * FROM orderheader
    WHERE orderid = @OrderId;
END

 CREATE PROCEDURE UpdateOrder
    @OrderId INT,
    @OrderNo VARCHAR(50)
AS
BEGIN
	 INSERT INTO orderheader (ordercompanyid,orderno,orderdate,jobtype,initiator,importer,exporter,referencesno,userid,isactive,isvoid)
    SELECT ordercompanyid,@OrderNo,orderdate,jobtype,initiator,importer,exporter,referencesno,userid,isactive,isvoid
    FROM orderheader
    WHERE orderid = @OrderId;
        
        UPDATE orderheader
        SET orderno = @OrderNo
        WHERE orderno=@OrderNo
    END
    

drop procedure UpdateOrder

create procedure updateinvoice
    @orderId int,
	@orderNo varchar(50)
as
begin
declare @orderinvoice int=(select orderid from orderheader where orderno=@orderNo)
insert into orderinvoice(orderid,invoiceno,invoicedate,invoicetype,currencycode,exchangerate,invoiceamount,invoicelocalvalue,inschargetype,insvalue,inslocalvalue,othchargetype,othvalue,othlocalvalue,costins,costoth,costinsoth,userid,isactive,isvoid)

select @orderinvoice,invoiceno,invoicedate,invoicetype,currencycode,exchangerate,invoiceamount,invoicelocalvalue,inschargetype,insvalue,inslocalvalue,othchargetype,othvalue,othlocalvalue,costins,costoth,costinsoth,userid,isactive,isvoid
from orderinvoice where orderid=@orderId

end
drop procedure updateinvoice

create procedure updateitem
    @orderId int,
	@orderNo varchar(50)
as
begin
declare @orderitem int=(select orderid from orderheader where orderno=@orderNo)

insert into itemsections(orderid,invoiceno,itemcode,itemdescription,quantity,itemamount,itemlocalvalue,unitprice,userid,isactive,isvoid)

select @orderitem,invoiceno,itemcode,itemdescription,quantity,itemamount,itemlocalvalue,unitprice,userid,isactive,isvoid
from itemsections where orderid=@orderId
end
drop procedure updateitem

create procedure invoicecopy
   @invoiceno varchar(55),
   @invoiceid int
as
begin
declare @orderid int=(select orderid from orderinvoice where invoiceid=@invoiceid)

declare @invoicenumber varchar(55)=(select invoiceno from orderinvoice where invoiceid=@invoiceid)

if @invoiceno!=@invoicenumber
begin

insert into orderinvoice(orderid,invoiceno,invoicedate,invoicetype,currencycode,exchangerate,invoiceamount,invoicelocalvalue,inschargetype,insvalue,inslocalvalue,othchargetype,othvalue,othlocalvalue,costins,costoth,costinsoth,userid,isactive,isvoid)

select @orderid,@invoiceno,invoicedate,invoicetype,currencycode,exchangerate,invoiceamount,invoicelocalvalue,inschargetype,insvalue,inslocalvalue,othchargetype,othvalue,othlocalvalue,costins,costoth,costinsoth,userid,isactive,isvoid
from orderinvoice where invoiceid=@invoiceid
end;
end
drop procedure invoicecopy

create procedure itemcopy
   @invoiceno varchar(55),
   @invoiceid int
as
begin
declare @orderid int=(select orderid from orderinvoice where invoiceid=@invoiceid)

declare @invoiceno1 varchar(55)=(select invoiceno from orderinvoice where invoiceid=@invoiceid and orderid=@orderid)

insert into itemsections(orderid,invoiceno,itemcode,itemdescription,quantity,itemamount,itemlocalvalue,unitprice,userid,isactive,isvoid)

select @orderid,@invoiceno,itemcode,itemdescription,quantity,itemamount,itemlocalvalue,unitprice,userid,isactive,isvoid 
 from itemsections where invoiceno=@invoiceno1 And orderid=@orderid
end
drop procedure itemcopy

create procedure itemordercopy
  @itemid int
as
begin

DECLARE @orderid1 INT, @InvoiceNo VARCHAR(55), @itemamount INT, @invoicevalue INT, @value INT;

 
  SELECT @orderid1 = orderid, @InvoiceNo = invoiceno
  FROM itemsections
  WHERE itemid = @itemid;

 
  SELECT @itemamount =sum(itemamount)
  FROM itemsections
  WHERE invoiceno = @InvoiceNo And orderid=@orderid1
  
  
  SELECT @invoicevalue = invoicelocalvalue
  FROM orderinvoice
  WHERE orderid = @orderid1 AND invoiceno = @InvoiceNo;

 
  SET @value = @invoicevalue - @itemamount;
                 
if @value >=@itemamount
begin
insert into itemsections(orderid,invoiceno,itemcode,itemdescription,quantity,itemamount,itemlocalvalue,unitprice,userid,isactive,isvoid)

select orderid,invoiceno,itemcode,itemdescription,quantity,itemamount,itemlocalvalue,unitprice,userid,isactive,isvoid
from itemsections where itemid=@itemid
end;
end

drop procedure itemordercopy


