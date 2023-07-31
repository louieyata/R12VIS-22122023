delete from dbo.Person

DBCC CHECKIDENT('dbo.Person', RESEED, 0);

delete from dbo.Vaccination

DBCC CHECKIDENT('dbo.Vaccination', RESEED, 0);