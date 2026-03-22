                 +--------------------+
                 |       Users        |
                 |--------------------|
                 | UserId (PK)        |
                 | Email              |
                 | PasswordHash       |
                 | FullName           |
                 | MonthlySalary      |
                 | IsActive           |
                 | Role               |
                 | CreatedAt          |
                 | UpdatedAt          |
                 +---------+----------+
                           |
                           |
            +--------------+--------------+
            |                             |
            |                             |
+-----------v-----------+      +----------v----------+
|       Categories      |      |        Bills        |
|-----------------------|      |---------------------|
| CategoryId (PK)       |      | BillId (PK)         |
| UserId (FK) ----------+----->| UserId (FK)         |
| Name                  |      | Name                |
| Type                  |      | Amount              |
| Icon                  |      | CategoryId (FK) ----+
| Color                 |      | Frequency           |
| IsSystem              |      | StartDate           |
| CreatedAt             |      | EndDate             |
+-----------+-----------+      | DayOfMonth          |
            |                  | DayOfWeek           |
            |                  | MonthOfYear         |
            |                  | IsActive            |
            |                  | CreatedAt           |
            |                  +----------+----------+
            |                             |
            |                             |
            |                             |
+-----------v-----------+      +----------v----------+
|       Expenses        |      |        Income       |
|-----------------------|      |---------------------|
| ExpenseId (PK)        |      | IncomeId (PK)       |
| UserId (FK) ----------+----->| UserId (FK)         |
| CategoryId (FK) ------+      | Name                |
| BillId (FK) ----------+----->| Amount              |
| Amount                |      | IncomeDate          |
| Description           |      | Type                |
| ExpenseDate           |      | CreatedAt           |
| CreatedAt             |      +---------------------+
+-----------------------+


CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
	[PasswordHash] [nvarchar](500) NOT NULL,
	[FullName] [nvarchar](150) NULL,
	[MonthlySalary] [decimal](18, 2) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Role] [nvarchar](50) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



CREATE TABLE Categories
(
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,

    UserId INT NULL,

    Name NVARCHAR(100) NOT NULL,

    Type NVARCHAR(20) NOT NULL,
    -- Expense / Bill / Income

    Icon NVARCHAR(50) NULL,
    Color NVARCHAR(20) NULL,

    IsSystem BIT NOT NULL DEFAULT 0,

    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT FK_Categories_User
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId)
);

CREATE TABLE Bills
(
    BillId INT IDENTITY(1,1) PRIMARY KEY,

    UserId INT NOT NULL,

    Name NVARCHAR(150) NOT NULL,

    Amount DECIMAL(18,2) NOT NULL,

    CategoryId INT NULL,

    Frequency NVARCHAR(20) NOT NULL,
    -- Daily
    -- Weekly
    -- Monthly
    -- Quarterly
    -- HalfYearly
    -- Yearly
    -- OneTime

    StartDate DATE NOT NULL,

    EndDate DATE NULL,

    DayOfMonth INT NULL,
    DayOfWeek INT NULL,
    MonthOfYear INT NULL,

    IsActive BIT NOT NULL DEFAULT 1,

    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT FK_Bills_User
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_Bills_Category
        FOREIGN KEY (CategoryId)
        REFERENCES Categories(CategoryId)
);


CREATE TABLE Expenses
(
    ExpenseId INT IDENTITY(1,1) PRIMARY KEY,

    UserId INT NOT NULL,

    CategoryId INT NULL,

    BillId INT NULL,

    Amount DECIMAL(18,2) NOT NULL,

    Description NVARCHAR(250) NULL,

    ExpenseDate DATE NOT NULL,

    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT FK_Expenses_User
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_Expenses_Category
        FOREIGN KEY (CategoryId)
        REFERENCES Categories(CategoryId),

    CONSTRAINT FK_Expenses_Bill
        FOREIGN KEY (BillId)
        REFERENCES Bills(BillId)
);


CREATE TABLE Income
(
    IncomeId INT IDENTITY(1,1) PRIMARY KEY,

    UserId INT NOT NULL,

    Name NVARCHAR(150) NOT NULL,

    Amount DECIMAL(18,2) NOT NULL,

    IncomeDate DATE NOT NULL,

    Type NVARCHAR(50) NOT NULL,
    -- Salary
    -- Bonus
    -- Freelance
    -- Other

    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT FK_Income_User
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId)
);


CREATE INDEX IX_Expenses_User_Date
ON Expenses(UserId, ExpenseDate);

CREATE INDEX IX_Bills_User
ON Bills(UserId);

CREATE INDEX IX_Income_User_Date
ON Income(UserId, IncomeDate);

INSERT INTO Categories (UserId, Name, Type, IsSystem)
VALUES (1, 'Rent', 'Bill', 1);

INSERT INTO Categories (UserId, Name, Type, IsSystem)
VALUES (1, 'Food', 'Expense', 1);

INSERT INTO Categories (UserId, Name, Type, IsSystem)
VALUES (1, 'Subscription', 'Bill', 1);

INSERT INTO Categories (UserId, Name, Type, IsSystem)
VALUES (1, 'Salary', 'Income', 1);



INSERT INTO Bills
(
    UserId,
    Name,
    Amount,
    Frequency,
    StartDate,
    DayOfMonth,
    IsActive
)
VALUES
(
    1,
    'House Rent',
    15000,
    'Monthly',
    '2024-01-01',
    5,
    1
);


INSERT INTO Bills
(
    UserId,
    Name,
    Amount,
    Frequency,
    StartDate,
    DayOfMonth,
    IsActive
)
VALUES
(
    1,
    'Netflix',
    799,
    'Monthly',
    '2024-01-01',
    10,
    1
);



INSERT INTO Bills
(
    UserId,
    Name,
    Amount,
    Frequency,
    StartDate,
    MonthOfYear,
    IsActive
)
VALUES
(
    1,
    'Insurance',
    12000,
    'Yearly',
    '2024-01-01',
    6,
    1
);

INSERT INTO Expenses
(
    UserId,
    Amount,
    Description,
    ExpenseDate
)
VALUES
(
    1,
    500,
    'Food order',
    GETDATE()
);

INSERT INTO Expenses
(
    UserId,
    Amount,
    Description,
    ExpenseDate
)
VALUES
(
    1,
    1200,
    'Amazon',
    GETDATE()
);


INSERT INTO Income
(
    UserId,
    Name,
    Amount,
    IncomeDate,
    Type
)
VALUES
(
    1,
    'Salary',
    120000,
    GETDATE(),
    'Salary'
);


