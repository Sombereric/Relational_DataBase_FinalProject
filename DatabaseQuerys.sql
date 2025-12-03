-- creation of the database
CREATE DATABASE SchoolDataBase;
-- used to allow all changes to apply to only schoolDataBase
USE SchoolDataBase;

-- creation of each table 
CREATE TABLE Program(
	ProgramID INT NOT NULL,
    ProgramName VARCHAR(32),
    ProgramType VARCHAR(32)
);

CREATE TABLE Student(
	StudentID INT NOT NULL,
    FirstName VARCHAR(32),
    LastName VARCHAR(32),
    Address VARCHAR(32),
    PhoneNumber INT,
    DateOfBirth VARCHAR(32),
    ProgramID INT NOT NULL
);

CREATE TABLE EmergencyContact(
	ContactID INT NOT NULL,
    StudentID INT NOT NULL,
    ContactName VARCHAR(32),
    ContactAddress VARCHAR(32),
    ContactPhoneNumber VARCHAR(32),
    RelationshipToStudent VARCHAR(32)
);

CREATE TABLE Enrollment(
	EnrollmentID INT NOT NULL,
    StudentID INT NOT NULL,
    CourseID INT NOT NULL, 
    Term INT,
    Grade INT
);

CREATE TABLE Course(
	CourseID INT NOT NULL,
    Section INT,
    CourseName VARCHAR(32)
);

-- All Primary key creations
ALTER TABLE Program
ADD PRIMARY KEY (ProgramID);

ALTER TABLE Student
ADD PRIMARY KEY (StudentID);

ALTER TABLE EmergencyContact
ADD PRIMARY KEY (ContactID);

ALTER TABLE Course
ADD PRIMARY KEY (CourseId);

ALTER TABLE Enrollment
ADD PRIMARY KEY (EnrollmentID);

-- All Foreign Key creations to connect all tables to one another
ALTER TABLE Enrollment
ADD FOREIGN KEY (StudentID)
REFERENCES Student(StudentID);

ALTER TABLE Enrollment
ADD FOREIGN KEY (CourseId)
REFERENCES Course(CourseID);

ALTER TABLE EmergencyContact
ADD FOREIGN KEY (StudentID)
REFERENCES Student(StudentID);

ALTER TABLE Student
ADD foreign key (ProgramID)
REFERENCES Program(ProgramID);