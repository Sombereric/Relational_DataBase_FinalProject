-- creation of the database
CREATE DATABASE SchoolDataBase;
-- used to allow all changes to apply to only schoolDataBase
USE SchoolDataBase;

-- creation of each table 
CREATE TABLE Program(
    ProgramID INT NOT NULL,
    ProgramName VARCHAR(32),
    ProgramType VARCHAR(32),
    PRIMARY KEY (ProgramID)
);

CREATE TABLE Student(
    StudentID INT NOT NULL,
    FirstName VARCHAR(32),
    LastName VARCHAR(32),
    Address VARCHAR(32),
    PhoneNumber INT,
    DateOfBirth VARCHAR(32),
    ProgramID INT NOT NULL,
    PRIMARY KEY (StudentID)
);

CREATE TABLE EmergencyContact(
    ContactID INT NOT NULL,
    StudentID INT NOT NULL,
    ContactName VARCHAR(32),
    ContactAddress VARCHAR(32),
    ContactPhoneNumber VARCHAR(32),
    RelationshipToStudent VARCHAR(32),
    PRIMARY KEY (ContactID)
);

CREATE TABLE Course(
    CourseID INT NOT NULL,
    Section INT,
    CourseName VARCHAR(32),
    PRIMARY KEY (CourseID)
);

CREATE TABLE Enrollment(
    EnrollmentID INT NOT NULL,
    StudentID INT NOT NULL,
    CourseID INT NOT NULL,
    Term INT,
    Grade INT,
    PRIMARY KEY (EnrollmentID)
);

-- Foreign keys with cascading deletes (and cascading updates)
ALTER TABLE Enrollment
  ADD CONSTRAINT fk_enrollment_student
    FOREIGN KEY (StudentID)
    REFERENCES Student(StudentID)
    ON DELETE CASCADE
    ON UPDATE CASCADE;

ALTER TABLE Enrollment
  ADD CONSTRAINT fk_enrollment_course
    FOREIGN KEY (CourseID)
    REFERENCES Course(CourseID)
    ON DELETE CASCADE
    ON UPDATE CASCADE;

ALTER TABLE EmergencyContact
  ADD CONSTRAINT fk_emergencycontact_student
    FOREIGN KEY (StudentID)
    REFERENCES Student(StudentID)
    ON DELETE CASCADE
    ON UPDATE CASCADE;

ALTER TABLE Student
  ADD CONSTRAINT fk_student_program
    FOREIGN KEY (ProgramID)
    REFERENCES Program(ProgramID)
    ON DELETE RESTRICT    -- prevents deleting a program while students exist
    ON UPDATE CASCADE;

-- This is where the test data created

INSERT INTO Program (ProgramID, ProgramName, ProgramType) VALUES
(1, 'Computer Science', 'Diploma'),
(2, 'Business Admin', 'Diploma'),
(3, 'Nursing', 'Degree'),
(4, 'Electrical Engineering', 'Degree'),
(5, 'Culinary Arts', 'Certificate');

INSERT INTO Student (StudentID, FirstName, LastName, Address, PhoneNumber, DateOfBirth, ProgramID) VALUES
(101, 'Eric', 'Moutoux', '123 Maple St', 5551234, '2001-06-15', 1),
(102, 'Sarah', 'Li', '99 Oak Ave', 5552345, '2002-09-21', 2),
(103, 'John', 'Smith', '45 Birch Rd', 5553456, '2000-02-11', 1),
(104, 'Ava', 'Martinez', '87 Pine St', 5554567, '2003-12-05', 3),
(105, 'Michael', 'Brown', '12 Cedar Dr', 5555678, '2001-03-29', 4),
(106, 'Emily', 'Johnson', '7 River Way', 5556789, '2002-11-08', 5),
(107, 'Daniel', 'Cho', '120 Forest Ln', 5557890, '2000-08-02', 1);

INSERT INTO Course (CourseID, Section, CourseName) VALUES
(301, 1, 'Intro to Programming'),
(302, 2, 'Data Structures'),
(303, 1, 'Business Accounting'),
(304, 3, 'Human Anatomy'),
(305, 1, 'Electrical Theory'),
(306, 2, 'Culinary Fundamentals'),
(307, 1, 'Operating Systems');

INSERT INTO Enrollment (EnrollmentID, StudentID, CourseID, Term, Grade) VALUES
(401, 101, 301, 1, 88),
(402, 101, 302, 2, 92),
(403, 103, 301, 1, 77),
(404, 104, 304, 1, 85),
(405, 105, 305, 1, 81),
(406, 106, 306, 1, 90),
(407, 107, 301, 1, 73),
(408, 102, 303, 1, 95),
(409, 103, 307, 2, 84),
(410, 107, 302, 2, 79);

INSERT INTO EmergencyContact (ContactID, StudentID, ContactName, ContactAddress, ContactPhoneNumber, RelationshipToStudent) VALUES
(201, 101, 'Marie Moutoux', '123 Maple St', '555-9876', 'Mother'),
(202, 102, 'Wei Li', '99 Oak Ave', '555-7654', 'Father'),
(203, 103, 'Anna Smith', '45 Birch Rd', '555-6543', 'Mother'),
(204, 104, 'Carlos Martinez', '87 Pine St', '555-5432', 'Brother'),
(205, 105, 'Linda Brown', '12 Cedar Dr', '555-4321', 'Mother'),
(206, 106, 'Thomas Johnson', '7 River Way', '555-3210', 'Father'),
(207, 107, 'Grace Cho', '120 Forest Ln', '555-2109', 'Sister');