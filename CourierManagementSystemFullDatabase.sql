Create DataBase MyPractiseOne
Go

USE MyPractiseOne
GO

Create Table  CountryMaster
(
CountryID Numeric(18,0) Identity,
CountryCode Varchar(10) Not Null,
CountryName Varchar(50), 
IsActive Bit,
CreatedBy Varchar(10),
CreatedDate DateTime,
ModifiedBy Varchar(10),
ModifiedDate DateTime
)
Go

ALTER TABLE CountryMaster
ADD PRIMARY KEY (CountryCode);
Go

ALTER PROC Sp_CountryMaster_InsertUpdate
(
  @CountryID Numeric(18,0),
	@CountryCode Varchar(10),
	@CountryName Varchar(50),
	@IsActive Bit
)
AS
	BEGIN
		Declare @Msg Varchar(Max)='',@Status Int=0,@Focus Varchar(50)='txtCountryCode'
		If isnull(@CountryCode,'')=''
		Begin
			Set @Msg='Please Enter Country Code..!'
		End		
		ELSE IF EXISTS(Select CountryCode From CountryMaster Where CountryCode=@CountryCode)
		Set @Msg='Country Code Already Exist.'
		Else IF @CountryID=0
		Begin
			INSERT INTO CountryMaster(CountryCode,CountryName,IsActive,CreatedDate)
			VALUES(@CountryCode,@CountryName,@IsActive,GETDATE())
			set @CountryID= SCOPE_IDENTITY()
			If @CountryID >0
			Begin
				Set @Status=1	
				Set @Msg='Record Inserted Successfully..!'
			End
		End
	Else
	Begin
		UPDATE CountryMaster SET CountryCode=@CountryCode,CountryName=@CountryName,IsActive=@IsActive,ModifiedDate=GETDATE() 
		WHERE CountryID=@CountryID
		Set @Status=2
		Set @Msg='Record Updated Successfully..!'
	End
	Select @Msg As Result,@Status As [Status]
End
GO

CREATE PROC Sp_CountryMaster_Edit
(
@CountryID Numeric(18,0)
)
AS
	BEGIN
		Select * From CountryMaster Where CountryID=@CountryID
	END
Go

CREATE PROC Sp_CityMaster_Edit 5
(
@CityID Numeric(18,0)
)
AS
	BEGIN
		Select * From CityMaster Where CityID=@CityID
	END
Go

CREATE PROC Sp_CountryMaster_Delete
(
@CountryID Numeric(18,0)
)
AS
	BEGIN
		Delete  From CountryMaster Where CountryID=@CountryID
	END
Go

CREATE PROC Sp_StateMaster_Delete
(
@StateID Numeric(18,0)
)
AS
	BEGIN
		Delete  From StateMaster Where StateID=@StateID
	END
Go

CREATE PROC Sp_CityMaster_Delete
(
@CityID Numeric(18,0)
)
AS
	BEGIN
		Delete  From CityMaster Where CityID=@CityID
	END
Go

CREATE PROC Sp_CountryMaster_Get
AS
	BEGIN
		Select * From CountryMaster 
	END
Go

Create Table  StateMaster
(
StateID Numeric(18,0) Identity,
StateCode Varchar(10) Not Null Primary Key,
StateName Varchar(50), 
CountryCode Varchar(10) Foreign Key References CountryMaster(CountryCode),
IsActive Bit,
CreatedBy Varchar(10),
CreatedDate DateTime,
ModifiedBy Varchar(10),
ModifiedDate DateTime
)
Go

ALTER PROC Sp_StateMaster_InsertUpdate 
(
	@StateID Numeric(18,0),
	@StateCode Varchar(10),
	@StateName Varchar(50),
	@CountryCode Varchar(10),
	@IsActive Bit
)
AS
	BEGIN
		Declare @Msg Varchar(Max)='',@Status Int=0,@Focus Varchar(50)='txtStateCode'
		If isnull(@StateCode,'')=''
		Begin
			Set @Msg='Please Enter State Code..!'
		End		
		ELSE IF EXISTS(Select StateCode From StateMaster Where StateCode=@StateCode)
		Set @Msg='State Code Already Exist.'
		Else IF @StateID=0
		Begin
			INSERT INTO StateMaster(StateCode,StateName,CountryCode,IsActive,CreatedDate)
			VALUES(@StateCode,@StateName,@CountryCode,@IsActive,GETDATE())
			set @StateID= SCOPE_IDENTITY()
			If @StateID >0
			Begin
				Set @Status=1	
				Set @Msg='Record Inserted Successfully..!'
			End
		End
	Else
	Begin
		UPDATE StateMaster SET StateCode=@StateCode,StateName=@StateName,CountryCode=@CountryCode,IsActive=@IsActive,ModifiedDate=GETDATE() 
		WHERE StateID=@StateID
		Set @Status=2
		Set @Msg='Record Updated Successfully..!'
	End
	Select @Msg As Result,@Status As [Status]
End
GO

ALTER PROC Sp_StateMaster_GetCountryCode
AS
	BEGIN
		Select CountryCode,CountryName From CountryMaster 
	END
Go

ALTER PROC Sp_CityMaster_GetStateCode 
(
@CountryCode Varchar(10)
)
AS
	BEGIN
		Select StateCode,StateName,CountryCode From StateMaster where CountryCode=@CountryCode
	END
Go

Create Table  CityMaster
(
CityID Numeric(18,0) Identity,
CityCode Varchar(10) Not Null Primary Key,
CityName Varchar(50), 
CountryCode Varchar(10) Foreign Key References CountryMaster(CountryCode),
StateCode Varchar(10) Foreign Key References StateMaster(StateCode),
IsActive Bit,
CreatedBy Varchar(10),
CreatedDate DateTime,
ModifiedBy Varchar(10),
ModifiedDate DateTime
)
Go

ALTER TABLE CityMaster
DROP COLUMN CityCode;
GO

ALTER TABLE CityMaster
ADD PRIMARY KEY (CityID);
Go

ALTER PROC Sp_CityMaster_GetCityCode 
(
@StateCode Varchar(10)
)
AS
	BEGIN
		Select *,StateCode From CityMaster where StateCode=@StateCode
	END
Go
ALTER PROC Sp_CityMaster_InsertUpdate 
(
	@CityID Numeric(18,0),

	@CityName Varchar(50),
	@CountryCode Varchar(10),
	@StateCode Varchar(10),
	@IsActive Bit
)
AS
	BEGIN
		Declare @Msg Varchar(Max)='',@Status Int=0,@Focus Varchar(50)='txtCityCode'
		
		 IF EXISTS(Select CityName From CityMaster Where CityName=@CityName)
		Set @Msg='City Name Already Exist.'
		Else IF @CityID=0
		Begin
			INSERT INTO CityMaster(CityName,CountryCode,StateCode,IsActive,CreatedDate)
			VALUES(@CityName,@CountryCode,@StateCode,@IsActive,GETDATE())
			set @CityID= SCOPE_IDENTITY()
			If @CityID >0
			Begin
				Set @Status=1	
				Set @Msg='Record Inserted Successfully..!'
			End
		End
	Else
	Begin
		UPDATE CityMaster SET CityName=@CityName,CountryCode=@CountryCode,StateCode=@StateCode,IsActive=@IsActive,ModifiedDate=GETDATE() 
		WHERE CityID=@CityID
		Set @Status=2
		Set @Msg='Record Updated Successfully..!'
	End
	Select @Msg As Result,@Status As [Status]
End
GO

ALTER TABLE CityMaster
DROP CONSTRAINT PK__CityMast__B488218D3FB0FB5B;
GO

CREATE PROC Sp_StateMaster_Get
AS
	BEGIN
		Select * From StateMaster 
	END
Go

CREATE PROC Sp_CityMaster_GetByID
@CityID NUMERIC(18,0)
AS
	BEGIN
		Select * From CityMaster WHERE CityID=@CityID
	END
Go

Alter PROC Sp_CityMaster_Get
AS
	BEGIN
		Select CTM.CityID,CTM.CityName AS [City],(CTM.CountryCode+' : '+CM.CountryName) AS Country,(CTM.StateCode+' : '+SM.StateName) AS [State],
		(CASE WHEN CTM.IsActive = 1 THEN 'YES' ELSE 'NO' END) AS [Is Active]
		,CTM.CreatedBy AS [CREATED BY],CONVERT(varchar(50),CTM.CreatedDate,106) AS [Created Date],CTM.ModifiedBy AS [Modified By],CTM.ModifiedDate AS [Modified Date] 
		FROM CityMaster CTM
		Left join CountryMaster CM on CTM.CountryCode=CM.CountryCode 
		Left join StateMaster SM on CTM.StateCode=SM.StateCode
	END
Go

ALTER TABLE CityMaster ADD [Action] VARCHAR(100) NULL

CREATE PROC Sp_CityMaster_GetALL
AS
	BEGIN
		Select * From CityMaster 
	END
Go



Create Table  PincodeMaster
(
PincodeID Numeric(18,0) Identity Primary Key,
PinCode Varchar(6),
CityID Numeric(18,0) Foreign Key References CityMaster(CityID), 
CountryCode Varchar(10) Foreign Key References CountryMaster(CountryCode),
StateCode Varchar(10) Foreign Key References StateMaster(StateCode),
IsActive Bit,
CreatedBy Varchar(10),
CreatedDate DateTime,
ModifiedBy Varchar(10),
ModifiedDate DateTime
)
Go


Create PROC Sp_PincodeMaster_InsertUpdate 
(
	@PincodeID Numeric(18,0),
	@PinCode Varchar(6),
	@CityID Numeric(18,0),
	@CountryCode Varchar(10),
	@StateCode Varchar(10),
	@IsActive Bit
)
AS
	BEGIN
		Declare @Msg Varchar(Max)='',@Status Int=0,@Focus Varchar(50)='txtPinCode'
		IF EXISTS(Select PinCode From PincodeMaster Where PinCode=@PinCode)
		Set @Msg='Pin Code Already Exist.'
		Else IF @PincodeID=0
		Begin
			INSERT INTO PincodeMaster(PinCode,CityID,CountryCode,StateCode,IsActive,CreatedDate)
			VALUES(@PinCode,@CityID,@CountryCode,@StateCode,@IsActive,GETDATE())
			set @PincodeID= SCOPE_IDENTITY()
			If @PincodeID >0
			Begin
				Set @Status=1	
				Set @Msg='Record Inserted Successfully..!'
			End
		End
	Else
	Begin
		UPDATE PincodeMaster SET PinCode=@PinCode,CityID=@CityID,CountryCode=@CountryCode,StateCode=@StateCode,IsActive=@IsActive,ModifiedDate=GETDATE() 
		WHERE PincodeID=@PincodeID
		Set @Status=2
		Set @Msg='Record Updated Successfully..!'
	End
	Select @Msg As Result,@Status As [Status]
End
GO

SELECT * FROM PincodeMaster
go

ALTER PROC Sp_PincodeMaster_Get
AS
	BEGIN
		Select PM.PincodeID,PM.PinCode AS [Pin Code],(CTM.CityName) AS City,(CM.CountryName) AS Country,(SM.StateName) AS [State],
		(CASE WHEN PM.IsActive = 1 THEN 'YES' ELSE 'NO' END) AS [Is Active]
		,PM.CreatedBy AS [CREATED BY],CONVERT(varchar(50),PM.CreatedDate,106) AS [Created Date],PM.ModifiedBy AS [Modified By],PM.ModifiedDate AS [Modified Date] 
		FROM PincodeMaster PM
		Left join CountryMaster CM on PM.CountryCode=CM.CountryCode 
		Left join StateMaster SM on PM.StateCode=SM.StateCode
		Left Join CityMaster CTM on PM.CityID=CTM.CityID
	END
Go

CREATE PROC Sp_PincodeMaster_AddressByPincode
@PinCode Varchar(6)
AS
	BEGIN
		Select PincodeMaster.CountryCode,CountryMaster.CountryName,PincodeMaster.StateCode,StateMaster.StateName,PincodeMaster.CityID,CityMaster.CityName From PincodeMaster
		Inner Join CountryMaster On PincodeMaster.CountryCode=CountryMaster.CountryCode 
		Inner Join StateMaster On PincodeMaster.StateCode=StateMaster.StateCode
		Inner Join CityMaster On PincodeMaster.CityID=CityMaster.CityID
		Where PinCode=@PinCode
	END
Go



ALTER PROC Sp_PincodeMaster_Delete
(
@PincodeID Numeric(18,0)
)
AS
	BEGIN
		Delete  From PincodeMaster Where PincodeID=@PincodeID
	END
GO

ALTER PROC Sp_PincodeMaster_Edit
(
@PincodeID Numeric(18,0)
)
AS
	BEGIN
		Select *,CAST ( CityID AS INT ) AS CityCode From PincodeMaster Where PincodeID=@PincodeID
	END
GO

CREATE TABLE [dbo].[tblFiles](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [varchar](50) NULL,
    [ContentType] [varchar](50) NULL,
    [Data] [varbinary](max) NULL
)
GO

CREATE PROC Sp_Image_Insert
(
@Name VARCHAR(50),
@ContentType VARCHAR(50),
@Data VARBINARY (MAX)
)
AS
BEGIN
	INSERT INTO tblFiles(Name,ContentType,Data) VALUES (@Name,@ContentType,@Data)
END
GO

CREATE PROC Sp_Image_GetByID 
(
@Id INT
)
AS
BEGIN
	SELECT Data FROM tblFiles WHERE Id=@Id
END
GO

ALTER PROC Sp_Image_GetALL
AS
BEGIN
	SELECT * FROM tblFiles 
END
GO

Create Table  Packet
(
PacketID Numeric(18,0) Identity,
AWBNo Varchar(20) Primary Key,
ConsigerName Varchar(50),
ConsigerAddress Varchar(250),
ConsigerCountryCode Varchar(10),
ConsigerStateCode Varchar(10),
ConsigerCityName Varchar(50), 
ConsigerPincode Varchar(6), 
ConsigerPhoneNo Varchar(12),
ConsigerGSTNo Varchar(15),
ConsigerFaxNo Varchar(12),
ConsigneeName Varchar(50),
ConsigneeAddress Varchar(250),
ConsigneeCountryCode Varchar(10),
ConsigneeStateCode Varchar(10),
ConsigneeCityName Varchar(50), 
ConsigneePincode Varchar(6), 
ConsigneePhoneNo Varchar(12),
ConsigneeGSTNo Varchar(15),
ConsigneeFaxNo Varchar(12),

)
Go

ALTER TABLE PACKET
DROP COLUMN XMLData

ALTER TABLE PACKET
DROP COLUMN PacketWidth

ALTER TABLE PACKET
DROP COLUMN PacketLength

ALTER TABLE PACKET
DROP COLUMN PacketWeight

ALTER TABLE PACKET
DROP COLUMN PacketType


ALTER TABLE PACKET
DROP COLUMN PacketName

ALTER TABLE Packet
ADD CONSTRAINT PK_Packet
PRIMARY KEY(PacketID)

ALTER TABLE Packet
add XMLData Xml 

ALTER PROC Sp_Packet_InsertUpdate 
(
@PacketID numeric(18, 0),
@AWBNo varchar(20),
@ConsigerName varchar(50),
@ConsigerAddress varchar(250),
@ConsigerCountryCode varchar(10),
@ConsigerStateCode varchar(10),
@ConsigerCityName varchar(50),
@ConsigerPincode varchar(6),
@ConsigerPhoneNo varchar(12),
@ConsigerGSTNo varchar(15),
@ConsigerFaxNo varchar(12),
@ConsigneeName varchar(50),
@ConsigneeAddress varchar(250),
@ConsigneeCountryCode varchar(10),
@ConsigneeStateCode varchar(10),
@ConsigneeCityName varchar(50),
@ConsigneePincode varchar(6),
@ConsigneePhoneNo varchar(12),
@ConsigneeGSTNo varchar(15),
@ConsigneeFaxNo varchar(12),
@PacketDetails XML=NULL
)
AS
BEGIN
  DECLARE @Msg varchar(max) = '',
          @Status int = 0,
          @ID numeric(18, 0) = 0,
          @XMLData xml
  	 
  IF ISNULL(@ConsigerName, '') = ''
    SET @Msg = 'Please Enter Your Consiger Name'
  ELSE
  IF ISNULL(@ConsigerAddress, '') = ''
    SET @Msg = 'Please Enter Your Consiger Address'
  ELSE
  IF ISNULL(@ConsigerPincode, '') = ''
    SET @Msg = 'Please Enter Your Consiger Pincode'
  ELSE
  IF ISNULL(@ConsigerPhoneNo, '') = ''
    SET @Msg = 'Please Enter Your Consiger PhoneNo'
  ELSE
  IF ISNULL(@ConsigerGSTNo, '') = ''
    SET @Msg = 'Please Enter Your Consiger GSTNo'
  ELSE
  IF ISNULL(@ConsigerFaxNo, '') = ''
    SET @Msg = 'Please Select Your Consiger FaxNo'
  ELSE
  IF EXISTS (SELECT
      AWBNo
    FROM Packet
    WHERE AWBNo = @AWBNo)
    SET @Msg = 'AWB Number Already Exist.'
  ELSE
  IF @PacketID = 0
  BEGIN
    INSERT INTO Packet (AWBNo, ConsigerName, ConsigerAddress, ConsigerCountryCode,
    ConsigerStateCode, ConsigerCityName, ConsigerPincode, ConsigerPhoneNo, ConsigerGSTNo, ConsigerFaxNo, ConsigneeName,
    ConsigneeAddress, ConsigneeCountryCode, ConsigneeStateCode, ConsigneeCityName, ConsigneePincode, ConsigneePhoneNo, ConsigneeGSTNo, ConsigneeFaxNo)
      VALUES (@AWBNo, @ConsigerName, @ConsigerAddress, @ConsigerCountryCode, @ConsigerStateCode, @ConsigerCityName, @ConsigerPincode, @ConsigerPhoneNo, @ConsigerGSTNo, @ConsigerFaxNo, @ConsigneeName, @ConsigneeAddress, @ConsigneeCountryCode, @ConsigneeStateCode, @ConsigneeCityName, @ConsigneePincode, @ConsigneePhoneNo, @ConsigneeGSTNo, @ConsigneeFaxNo)

    SET @ID = SCOPE_IDENTITY();

    IF @ID > 0
    BEGIN
      SET @Status = 1
      SET @Msg = 'Record Saved Successfully'

	  
	INSERT INTO PacketStatusMaster(PacketID,StatusDate,StatusTime,Location,PacketStatus,CreteadBy,CreatedDate)
VALUES(@ID,CONVERT(VARCHAR(20),GETDATE(),106),RIGHT(CONVERT(VARCHAR, GETDATE(), 100),7),@ConsigerAddress,'InTransit','XYZ',CONVERT(VARCHAR(20),GETDATE(),106))


    END
	If @PacketDetails is not null
	begin
    --SELECt @xmlData = BulkColumn FROM OPENROWSET(BULK 'E:\Training\MyPractiseOne\MyPractiseOne\UploadImage\pp.xml', SINGLE_CLOB) AS a

	INSERT INTO PacketDetails (PacketName, PacketType, PacketWeight, PacketLength, PacketWidth, PacketHeight, PacketID)

      SELECT
        PacketName = x.a.value('(PacketName/text())[1]', 'VARCHAR(100)'),
        PacketType = x.a.value('(PacketType/text())[1]', 'VARCHAR(100)'),
        PacketWeight = x.a.value('(Weight/text())[1]', 'VARCHAR(100)'),
        PacketLength = x.a.value('(Length/text())[1]', 'VARCHAR(100)'),
        PacketWidth = x.a.value('(Width/text())[1]', 'VARCHAR(100)'),
        PacketHeight = x.a.value('(Height/text())[1]', 'VARCHAR(100)'),
        PacketID = @ID
      FROM @PacketDetails.nodes('/DocumentElement/PacketDetails') x (a)
	  end
	  end
	


  ELSE
  BEGIN
    UPDATE Packet
    SET AWBNo = @AWBNo,
        ConsigerName = @ConsigerName,
        ConsigerAddress = @ConsigerAddress,
        ConsigerCountryCode = @ConsigerCountryCode,
        ConsigerStateCode = @ConsigerStateCode,
        ConsigerCityName = @ConsigerCityName,
        ConsigerPincode = @ConsigerPincode,
        ConsigerPhoneNo = @ConsigerPhoneNo,
        ConsigerGSTNo = @ConsigerGSTNo,
        ConsigerFaxNo = @ConsigerFaxNo,
        ConsigneeName = @ConsigneeName,
        ConsigneeAddress = @ConsigneeAddress,
        ConsigneeCountryCode = @ConsigneeCountryCode,
        ConsigneeStateCode = @ConsigneeStateCode,
        ConsigneeCityName = @ConsigneeCityName,
        ConsigneePincode = @ConsigneePincode,
        ConsigneePhoneNo = @ConsigneePhoneNo,
        ConsigneeGSTNo = @ConsigneeGSTNo,
        ConsigneeFaxNo = @ConsigneeFaxNo
    WHERE PacketID = @PacketID
    SET @Status = 2
    SET @Msg = 'Record Updated Successfully'
  END
  SELECT
    @Msg AS Result,
    @Status AS [Status]
END
GO

ALTER PROC Sp_PacketAllDetails_Get
AS
	BEGIN
		SELECT Packet.PacketID AS [Packet ID], Packet.AWBNo AS [AWB No],Packet.ConsigerName AS [Consiger Name],
		Packet.ConsigerAddress AS [Consiger Address],Packet.ConsigerPhoneNo AS [Consiger PhoneNo],
		Packet.ConsigerGSTNo AS [Consiger GSTNo],Packet.ConsigneeName As [Consignee Name],
				Packet.ConsigneeAddress AS [Consignee Address],Packet.ConsigneePhoneNo AS [Consignee PhoneNo],
		Packet.ConsigneeGSTNo AS [Consignee GSTNo]    
		From Packet
	
	
	END
GO


Create Table  PacketDetails
(
PacketID Numeric(18,0) Identity Primary Key,
PacketName Varchar(50),
PacketType Varchar(20),
PacketWeight Numeric(10,3),
PacketLength Numeric(10,3),
PacketWidth Numeric(10,3),
PacketHeight Numeric(10,3),
)
Go

ALTER TABLE PacketDetails
alter COLUMN PacketWeight varchar(50)

ALTER TABLE PacketDetails
alter COLUMN PacketLength varchar(50)

ALTER TABLE PacketDetails
alter COLUMN PacketWidth varchar(50)
ALTER TABLE PacketDetails
alter COLUMN PacketHeight varchar(50)
PacketLength datatype;

ALTER TABLE PacketDetails
ADD PacketDetailsID Numeric(18,0) Identity Primary Key

ALTER TABLE PacketDetails
DROP constraint PK__PacketDe__E2BC75ED146B2039

ALTER TABLE PacketDetails
ADD FOREIGN KEY (PacketID) REFERENCES Packet(PacketID);

ALTER TABLE PacketDetails
    ADD PacketID Numeric(18,0),
     FOREIGN KEY(PacketID) REFERENCES Packet(PacketID);

ALTER TABLE PacketDetails
ADD CONSTRAINT FK_PacketDetails
FOREIGN KEY (PacketID) REFERENCES Packet(PacketID);

ALTER PROC Sp_PacketDetails_InsertUpdate
(
	@PacketName Varchar(50),
	@PacketType Varchar(20),
	@PacketWeight varchar(50),
	@PacketLength varchar(50),
	@PacketWidth varchar(50),
	@PacketHeight varchar(50),
    @PacketDetailsID Numeric(18,0),
    @PacketID Numeric(18,0)
)
AS
	BEGIN
	Declare @Msg Varchar(Max)='',@Status Int= 0,@ID Numeric(18,0)= 0
	If IsNull(@PacketName,'')=''
	Set @Msg='Please Enter Your Packet Name'
	Else If isnull(@PacketType,'')=''
	Set @Msg='Please Enter Your Packet Type'
	Else If isnull(@PacketWeight,'')=''
	Set @Msg='Please Enter Your Packet Weight'
	Else If isnull(@PacketLength,'')=''
	Set @Msg='Please Enter Your Packet Length'
	Else If isnull(@PacketWidth,'')=''
	Set @Msg='Please Enter Your Packet Width'
	Else If isnull(@PacketHeight,'')='' 
	Set @Msg='Please Select Your Packet Height'
	Else If @PacketDetailsID=0
	Begin
		Insert Into PacketDetails(PacketName,PacketType,PacketWeight,PacketLength,PacketWidth,PacketHeight,PacketID)
		Values(@PacketName,@PacketType,@PacketWeight,@PacketLength,@PacketWidth,@PacketHeight,@PacketID)
		Set @ID=SCOPE_IDENTITY();
		If @ID>0
		Begin
			Set @Status=1
			set @Msg='Record Saved Successfully'
		End 
	End
	Else
		Begin
			Update PacketDetails Set PacketName=@PacketName,PacketType=@PacketType,PacketWeight=@PacketWeight,PacketLength=@PacketLength,
			PacketWidth=@PacketWidth,PacketHeight=@PacketHeight
			Where PacketDetailsID=@PacketDetailsID
			Set @Status=2
			Set @Msg='Record Updated Successfully'
		End
	Select @Msg As Msg,@Status As [Status]
End
GO

Create Table  PacketStatusMaster
(
PacketStatusID Numeric(18,0) Identity Primary Key,
PacketID Numeric(18,0),
StatusDate Date,
StatusTime DateTime,
Location Varchar(50),
PacketStatus Varchar(40) DEFAULT 'IN Transit',
CreteadBy Varchar(50),
CreatedDate DateTime,
ModifiedBy Varchar(50),
ModifiedDate DateTime,
FOREIGN KEY(PacketID) REFERENCES Packet(PacketID),
)
Go


alter PROC Sp_PacketStatusMaster_InsertUpdate 
(
@PacketStatusID Numeric(18,0),
@PacketID Numeric(18,0),
@StatusDate Date,
@StatusTime DateTime,
@Location Varchar(50),
@PacketStatus Varchar(40),
@CreteadBy Varchar(50),
@CreatedDate DateTime,
@ModifiedBy Varchar(50),
@ModifiedDate DateTime
)
AS
	BEGIN
		Declare @Msg Varchar(Max)='',@Status Int=0,@Focus Varchar(50)='txtPinCode'
		
	   IF @PacketStatusID=0
		Begin
			INSERT INTO PacketStatusMaster(PacketID,StatusDate,StatusTime,Location,PacketStatus,CreteadBy,CreatedDate,ModifiedBy,ModifiedDate)
			VALUES(@PacketID,@StatusDate,@StatusTime,@Location,@PacketStatus,@CreteadBy,@CreatedDate,@ModifiedBy,@ModifiedDate)
			set @PacketStatusID= SCOPE_IDENTITY()
			If @PacketStatusID >0
			Begin
				Set @Status=1	
				Set @Msg='Record Inserted Successfully..!'
			End
		End
	Else
	Begin
		UPDATE PacketStatusMaster SET StatusDate=@StatusDate,StatusTime=@StatusTime,Location=@Location,PacketStatus=@PacketStatus,CreteadBy=@CreteadBy,CreatedDate=@CreatedDate, 
		ModifiedBy=@ModifiedBy,ModifiedDate=GETDATE()
		WHERE PacketStatusID=@PacketStatusID
		Set @Status=2
		Set @Msg='Record Updated Successfully..!'
	End
	Select @Msg As Result,@Status As [Status]
End
GO


ALTER PROC Sp_PacketStatusMaster_Get
AS
	BEGIN
		SELECT *, ISNULL(ModifiedBy,'') AS [NewModifiedBy] , REPLACE(ISNULL(CONVERT(DATE, ModifiedDate), ''), '1900-01-01', '') AS NewModifiedDate,CONVERT(VARCHAR(20),GETDATE(),106) AS [NewStatusDate],RIGHT(CONVERT(VARCHAR, StatusTime, 100),7) AS [NewStatusTime],CONVERT(VARCHAR(20),GETDATE(),106)AS [NewCreatedDate] FROM PacketStatusMaster
	END
GO



CREATE PROC Sp_PacketStatusMaster_Edit
(
@PacketStatusID Numeric(18,0)
)
AS
	BEGIN
		Select * From PacketStatusMaster Where PacketStatusID=@PacketStatusID
	END

	CREATE PROC Sp_PacketStatusMaster_Delete
(
@PacketStatusID Numeric(18,0)
)
AS
	BEGIN
		Delete  From PacketStatusMaster Where PacketStatusID=@PacketStatusID
	END

	
CREATE procedure [dbo].[Sp_PacketStatusMasterGetPacketID]  
@term varchar(50)  
as  
begin  
    select PacketID  
    from PacketStatusMaster  
    where PacketID like @term +'%'  
end  
GO  

alter procedure [dbo].[Sp_PacketStatusMasterGetAllRecordViaPacketID]  
@PacketID Numeric(18,0) 
as  
begin  
    select *, ISNULL(ModifiedBy,'') AS [NewModifiedBy] , REPLACE(ISNULL(CONVERT(DATE, ModifiedDate), ''), '1900-01-01', '') AS NewModifiedDate,CONVERT(VARCHAR(20),GETDATE(),106) AS [NewStatusDate],RIGHT(CONVERT(VARCHAR, StatusTime, 100),7) AS [NewStatusTime],CONVERT(VARCHAR(20),GETDATE(),106)AS [NewCreatedDate] 
    from PacketStatusMaster  
    where PacketID =@PacketID
end  
GO  

delete FROM PacketDetails
delete FROM packet
SELECT * FROM Packet
SELECT * FROM PacketDetails
SELECT * FROM PacketStatusMaster