--Store Procedure for 'Employee_Marital_Status' begins here


/* Store Procedure for inserting information */

CREATE Procedure prc_Insert_Into_MaritalStatus
@Marital_Status varchar(20),
@Created_By varchar(30)

AS
Begin
Set NOCOUNT ON;

INSERT INTO Employee_Marital_Status (Marital_Status,Created_By)

VALUES(
@Marital_Status, 
@Created_By 
)

end
Go

EXEC prc_Insert_Into_MaritalStatus @Marital_Status = 'Unmarried',@created_By = 'Harsh'
GO


/* Store Procedure for fetching information */

CREATE PROCEDURE prc_GetMaritalStatus
@Status_id int

AS
	Begin
	SET NOCOUNT ON;

SELECT Status_id,Marital_Status,
case when 
          Is_Active=1 then 'Active'
		  else 'Inactive' 
		  end 'Status'
from Employee_Marital_Status 
WHERE Status_id = @Status_id and Is_Deleted=0

end

GO

EXEC prc_GetMaritalStatus @Status_id=1
Go


/* Store Procedure for updating information */
CREATE PROCEDURE prc_UpdateMaritalStatus
@Status_id int,
@Updated_By varchar(30),
@Is_Active bit

AS
Begin

SET NOCOUNT ON;

Update Employee_Marital_Status 
SET Updated_By = @Updated_By, Updated_At = GETDATE(),Is_Active = @Is_Active 
WHERE Status_id = @Status_id
end
GO

--Getting current date here
declare @Up_At as datetime = getdate()
Exec prc_UpdateMaritalStatus @Updated_By = 'Harsh', @Is_Active = 1, @Status_Id = 1
Go


/* Store Procedure for deleting information */

CREATE PROCEDURE prc_DeleteMaritalStatus
@Status_id int

AS

	Begin
	SET NOCOUNT ON;

	UPDATE Employee_Marital_Status 
	SET Is_Deleted = 1 
	WHERE Status_id = @Status_id
end
Go
exec prc_DeleteMaritalStatus @Status_id = 3
Go

--Store Procedure for 'Employee_Marital_Status' ends here








--Store Procedure for 'employee_info' starts here


/* Store Procedure for inserting information */
CREATE PROCEDURE prc_InsertEmployee
@FirstName varchar(50),
@MiddleName varchar(50)=null,
@LastName varchar(50)=null,
@Age int=null,
@PhoneNo varchar(12)=null,
@MobileNo bigint,
@EmailId varchar(100),
@DOB date,
@MaritalStatus_emp int,
@Created_By varchar(30)

AS

BEGIN	
	Set NOCOUNT ON;

	INSERT INTO employee_info
	(FirstName, MiddleName, LastName, Age, DOB, PhoneNo, MobileNo, MaritalStatus_emp, EmailId,Created_By)
	VALUES
	(@FirstName,@MiddleName,@LastName,@Age,@DOB,@PhoneNo,@MobileNo,@MaritalStatus_emp,@EmailId,@Created_By)
END
Go


EXEC prc_InsertEmployee @FirstName='Harshpreet',
@LastName='Singh',
@Age=21,
@DOB='2000-11-08',
@MobileNo=8556924458,@MaritalStatus_emp=1,
@EmailId='harshpreetsingh277@gmail.com',
@Created_By='Harsh';
Go



/* Store Procedure for fetching information */
CREATE PROCEDURE prc_GetEmployee
@emp_id int


AS
BEGIN
Set NOCOUNT ON;

SELECT emp_id,FirstName,MiddleName,LastName,Age,MobileNo,PhoneNo,EmailId,DOB,b.Marital_Status,
case
      when a.Is_Active=1 then 'Active'
	  else 'Inactive' 
	  end 'Status'
 
FROM employee_info as a inner join emp_maritalStatus as b on b.Status_id=a.MaritalStatus_emp
where
       emp_id=@emp_id and a.Is_Deleted=0
END
Go

EXEC prc_GetEmployee @Emp_id=2
Go



/* Store Procedure for updating information */
CREATE PROCEDURE prc_UpdateEmployee
@emp_id int,
@MobileNo bigint,
@MaritalStatus_emp int,
@Updated_By varchar(30),
@Updated_At datetime,
@Is_Active bit
AS
BEGIN
Set NOCOUNT ON;

Update employee_info 
SET MobileNo=@MobileNo,MaritalStatus_emp=@MaritalStatus_emp,Updated_By=@Updated_By,Updated_At=GETDATE(),Is_Active=@Is_Active 
WHERE emp_id=@emp_id
END
Go
--Getting current date and time here
declare @EmployeeUpdatedAt 
as 
datetime=getdate()

EXEC prc_UpdateEmployee @MobileNo=8283905988,@MaritalStatus_emp=1,@Updated_By='Harsh',@updated_At=@EmployeeUpdatedAt,@Is_Active=1,@emp_id=2
Go


/* Store Procedure for deleting information */
CREATE PROCEDURE prc_DeleteEmployee
@emp_id int
AS
BEGIN
Set NOCOUNT ON;

UPDATE employee_info 
SET Is_Deleted = 1 
WHERE emp_id=@emp_id
END
Go

Exec prc_DeleteEmployee @emp_id=2
Go


