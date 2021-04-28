ALTER PROC UserAdd
@username varchar(50),
@password varchar(50)
AS
	INSERT INTO tbl_Login(username, password)
	VALUES (@username, @password)
