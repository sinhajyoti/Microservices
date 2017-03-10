drop table [order]

create table [Order](OrderId uniqueidentifier primary key, OrderName varchar(100), OrderAmount decimal(18,2), OrderStatus int, CorrelationId varchar(100), SequenceId int,
LastUpdatedby varchar(100), LastUpdatedOn datetime default getdate() )
--SequenceId is the sequence number of event with same correlationID that created this record

create table [OrderHist](OrderId uniqueidentifier primary key, OrderName varchar(100), OrderAmount decimal(18,2), OrderStatus int, CorrelationId varchar(100), SequenceId int,
LastUpdatedby varchar(100), LastUpdatedOn datetime default getdate() )

create table [OrderStatus](OrderStatusID int primary key, StatusShortDesc varchar(50), StatusLongDesc varchar(200))
