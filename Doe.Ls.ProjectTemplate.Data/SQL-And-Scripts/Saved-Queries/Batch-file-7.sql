DECLARE @tmp TABLE
(
DocNumber varchar( 500)
)


DECLARE @DocTable TABLE
(
DocNumber varchar( 500)
)

declare @tmpDocNumber varchar (400);

-- GENERATED from Excel

insert into @tmp VALUES ('DOC17/396755')
insert into @tmp VALUES ('DOC17/396755')
insert into @tmp VALUES ('DOC17/396755')
insert into @tmp VALUES ('DOC17/396755')
insert into @tmp VALUES ('DOC17/396755')
insert into @tmp VALUES ('DOC17/396755')
insert into @tmp VALUES ('DOC17/396755')
insert into @tmp VALUES ('DOC17/396755')
insert into @tmp VALUES ('DOC17/396755')
insert into @tmp VALUES ('DOC17/396755')
insert into @tmp VALUES ('DOC17/396755')
insert into @tmp VALUES ('DOC17/396755')
insert into @tmp VALUES ('DOC17/396755')
insert into @tmp VALUES ('DOC17/396755')
insert into @tmp VALUES ('DOC17/396755')
insert into @tmp VALUES ('DOC17/396755')
insert into @tmp VALUES ('DOC17/396755')
insert into @tmp VALUES ('DOC17/396755')
insert into @tmp VALUES ('DOC17/396755')
insert into @tmp VALUES ('DOC17/396755')

insert into @tmp VALUES ('DOC17/807399')
insert into @tmp VALUES ('DOC17/807409')
insert into @tmp VALUES ('DOC17/807409')
insert into @tmp VALUES ('DOC17/807409')
insert into @tmp VALUES ('DOC17/807409')
insert into @tmp VALUES ('DOC17/807409')
insert into @tmp VALUES ('DOC17/807409')
insert into @tmp VALUES ('DOC17/807409')
insert into @tmp VALUES ('DOC17/807409')
insert into @tmp VALUES ('DOC17/807409')
insert into @tmp VALUES ('DOC17/807409')
insert into @tmp VALUES ('DOC16/497604')
insert into @tmp VALUES ('DOC16/497604')
insert into @tmp VALUES ('DOC16/497604')
insert into @tmp VALUES ('DOC16/497604')
insert into @tmp VALUES ('DOC16/497604')
insert into @tmp VALUES ('DOC16/497604')


INSERT INTO @DocTable

SELECT DISTINCT DocNumber FROM @tmp




WHILE (SELECT COUNT(*) from @DocTable ) > 0
Begin

select Top 1 @tmpDocNumber=DocNumber from @DocTable order by DocNumber asc

select @tmpDocNumber;

delete from @DocTable where DocNumber = @tmpDocNumber

BEGIN TRY

print @tmpDocNumber
-- EXEC	 [dbo].[udp_BatchUpdateRdPdWithAllNonDeletedPositions]		@DocNumber = @tmpDocNumber,		@StatusId = 20

END TRY
BEGIN CATCH
select ERROR_MESSAGE ()
END CATCH
End

Go