-- Employee table
-- Students ID: 00013836, 00014725, 00014896

-- Get all
Go
Create or Alter Procedure pEmployee_GetAll
As
Begin
	Select Employee_ID, FName, LName, Telephone, Job, Age, Salary, HireDate, Image, FullTime
	From Employee
End

-- Get By Id
Go
Create or Alter Procedure pEmployee_GetById
    @Employee_ID int
As
Begin
    Select Employee_ID, FName, LName, Telephone, Job, Age, Salary, HireDate, Image, FullTime
    From Employee
    Where Employee_ID = @Employee_ID
End

-- Create
Go
Create or Alter Procedure pEmployee_Create
    @FName varchar(255),
    @LName varchar(255),
    @Telephone varchar(20),
    @Job varchar(255),
    @Age Int,
    @Salary Decimal(10, 2),
    @HireDate DateTime,
    @Image VarBinary(MAX),
    @FullTime BIT,
    @Errors nvarchar(1000) OUT
As
Begin
    If @Job Not In ('admin', 'cook', 'cashier')
    Begin 
        Raiserror('Invalid job type. Job type might consist of these: admin, cook, and cashier.', 15, 1)
        Return
    End

    Begin try
        Insert Into Employee (FName, LName, Telephone, Job, Age, Salary, HireDate, Image, FullTime)
        output inserted.Employee_ID
        Values (@FName, @LName, @Telephone, @Job, @Age, @Salary, @HireDate, @Image, @FullTime)
    
        return (1) -- 1 for success code
    End try
    Begin catch
        set @Errors = ERROR_MESSAGE()
        return (0) -- 0 for failure
    End catch
End

-- Update
Go
Create or Alter Procedure pEmployee_Update
  @EmployeeID int,
  @FName varchar(255),
  @LName varchar(255),
  @Telephone varchar(20),
  @Job varchar(255),
  @Age int,
  @Salary decimal(10, 2),
  @HireDate datetime,
  @Image varbinary(max),
  @FullTime bit,
  @Errors nvarchar(1000) out
As
Begin
  Begin try
    if @Job not in ('admin', 'cook', 'cashier')
    Begin 
      raiserror('Invalid job type. Job type might consist of these: admin, cook, and cashier.', 15, 1)
      return
    End

    Update Employee
    Set FName = @FName,
        LName = @LName,
        Telephone = @Telephone,
        Job = @Job,
        Age = @Age,
        Salary = @Salary,
        HireDate = @HireDate,
        Image = @Image,
        FullTime = @FullTime
    Where Employee_ID = @EmployeeID

    return (1) -- 1 for success code
  End try
  Begin catch
    set @Errors = ERROR_MESSAGE()
    return (0) -- 0 for failure
  End catch
End

-- Delete
Go
Create or Alter Procedure pEmployeeDelete
  @EmployeeID int,
  @Errors nvarchar(1000) out
As
Begin
  Begin try
    Delete from Employee
    where Employee_ID = @EmployeeID

    return (1) -- 1 for success code
  End try
  Begin catch
    Set @Errors = ERROR_MESSAGE()
    return (0) -- 0 for failure
  End catch
End